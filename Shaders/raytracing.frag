#version 430
out vec4 FragColor;
in 	vec3 glPosition;

uniform int RayTracingDepth;
uniform int CubeLoadedCount;
uniform vec3  LIGHT_POSITION;
int StackPTR = -1;
const vec3 Unit = vec3(1.0, 1.0, 1.0);
const float EPSILON = 0.0005;
const float BIG = 1.0 / 0.0;
const int STACK_LENGTH = 10;	
const int MATERIAL_DIFFUSE 	  = 1;
const int MATERIAL_REFLECTION = 2;
const int MATERIAL_REFRACTION = 3;
const int SPHERES_COUNT   = 3;
const int TRIANGLES_COUNT = 10;
const int MATERIAL_COUNT  = 6;
const int DIFFUSE_REFLECTION = 1;
const int MIRROR_REFLECTION  = 2;
const int CUBE_COUNT = 10;
const int CUBE_TRIANGLES_COUNT = 12;
const int CUBE_TOTAL_TRIANGLES_COUNT = CUBE_COUNT * CUBE_TRIANGLES_COUNT;
const vec3 RED    = vec3(1.0, 0.0, 0.0);
const vec3 GREEN  = vec3(0.0, 1.0, 0.0);
const vec3 BLUE   = vec3(0.0, 0.0, 1.0);
const vec3 YELLOW = vec3(1.0, 1.0, 0.0);
const vec3 WHITE  = vec3(1.0, 1.0, 1.0);

struct Cam 
{
	vec3 Position;
	vec3 View;
	vec3 Up;
	vec3 Side;
	vec2 Scale;
};
uniform Cam   uCamera;

struct SSphere 
{ 
	vec3  Center;
	float Radius;	
	int   MaterialIdx;
};
SSphere   	spheres	  [SPHERES_COUNT];

struct STriangle 
{
	vec3 v1;
	vec3 v2;
	vec3 v3;	
	int  MaterialIdx;
};
STriangle 	triangles [TRIANGLES_COUNT];
uniform STriangle CubeTriangles[CUBE_TOTAL_TRIANGLES_COUNT];

struct Ray 
{
	vec3 Origin;
	vec3 Direction;
};
struct SLight 
{
	vec3 Position;
};
SLight light;

struct SMaterial 
{ 
	vec3  Color;
	vec4  LightCoeffs;	
	float ReflectionCoef;
	float RefractionCoef;	
	int   MaterialType;
};
SMaterial 	materials [MATERIAL_COUNT];

struct STracingRay 
{
	Ray   ray;
	float contribution;	
	int   depth;
};
STracingRay Stack	  [STACK_LENGTH];

void initializeDefaultLightMaterials(out SLight light, out SMaterial materials[MATERIAL_COUNT])
{
	light.Position = LIGHT_POSITION;

	vec4 lightCoefs = vec4(0.4, 0.9, 0.0, 512.0);
	
	materials[0].Color 		 	= RED;
	materials[0].LightCoeffs 	= lightCoefs;
	materials[0].ReflectionCoef = 0.5;
	materials[0].RefractionCoef = 1.0;
	materials[0].MaterialType 	= MATERIAL_DIFFUSE;
	
	materials[1].Color 			= YELLOW;
	materials[1].LightCoeffs 	= lightCoefs;
	materials[1].ReflectionCoef = 0.5;
	materials[1].RefractionCoef = 1.0;
	materials[1].MaterialType 	= MATERIAL_DIFFUSE;
	
	materials[2].Color 			= BLUE;
	materials[2].LightCoeffs 	= lightCoefs;
	materials[2].ReflectionCoef = 0.5;
	materials[2].RefractionCoef = 1.0;
	materials[2].MaterialType 	= MATERIAL_DIFFUSE;
	
	materials[3].Color 			= GREEN;
	materials[3].LightCoeffs 	= lightCoefs;
	materials[3].ReflectionCoef = 0.5;
	materials[3].RefractionCoef = 1.0;
	materials[3].MaterialType 	= MATERIAL_DIFFUSE;
	
	materials[4].Color 			= WHITE;
	materials[4].LightCoeffs 	= lightCoefs;
	materials[4].ReflectionCoef = 0.5;
	materials[4].RefractionCoef = 1.0;
	materials[4].MaterialType 	= MATERIAL_DIFFUSE;
	
	
	materials[5].Color 			= WHITE;
	materials[5].LightCoeffs 	= lightCoefs;
	materials[5].ReflectionCoef = 1.0;
	materials[5].RefractionCoef = 0.5;
	materials[5].MaterialType 	= MATERIAL_REFLECTION;
}

void initializeDefaultScene( out STriangle triangles[TRIANGLES_COUNT], out SSphere spheres[SPHERES_COUNT] )
{		
	// left wall
	triangles[0].v1 = vec3(-5.0,-5.0,-5.0);
	triangles[0].v2 = vec3(-5.0, 5.0, 5.0);
	triangles[0].v3 = vec3(-5.0, 5.0,-5.0);
	triangles[0].MaterialIdx = 5;	
	triangles[1].v1 = vec3(-5.0,-5.0,-5.0);
	triangles[1].v2 = vec3(-5.0,-5.0, 5.0);
	triangles[1].v3 = vec3(-5.0, 5.0, 5.0);
	triangles[1].MaterialIdx = 5;
	
	// back wall  
	triangles[2].v1 = vec3(-5.0,-5.0, 5.0);
	triangles[2].v2 = vec3( 5.0,-5.0, 5.0);
	triangles[2].v3 = vec3(-5.0, 5.0, 5.0);
	triangles[2].MaterialIdx = 3;	
	triangles[3].v1 = vec3( 5.0, 5.0, 5.0);
	triangles[3].v2 = vec3(-5.0, 5.0, 5.0);
	triangles[3].v3 = vec3( 5.0,-5.0, 5.0);
	triangles[3].MaterialIdx = 3;
	
	// right wall  
	triangles[4].v1 = vec3( 5.0,-5.0,-5.0);
	triangles[4].v2 = vec3( 5.0, 5.0,-5.0);
	triangles[4].v3 = vec3( 5.0,-5.0, 5.0);
	triangles[4].MaterialIdx = 5;	
	triangles[5].v1 = vec3( 5.0, 5.0, 5.0);
	triangles[5].v2 = vec3( 5.0,-5.0, 5.0);
	triangles[5].v3 = vec3( 5.0, 5.0,-5.0);
	triangles[5].MaterialIdx = 5;
	
	// bottom wall  
	triangles[6].v1 = vec3(-5.0,-5.0, 5.0);
	triangles[6].v2 = vec3(-5.0,-5.0,-5.0);
	triangles[6].v3 = vec3( 5.0,-5.0, 5.0);
	triangles[6].MaterialIdx = 4;	
	triangles[7].v1 = vec3( 5.0,-5.0, 5.0);
	triangles[7].v2 = vec3(-5.0,-5.0,-5.0);
	triangles[7].v3 = vec3( 5.0,-5.0,-5.0);
	triangles[7].MaterialIdx = 4;
	
	// top wall  
	triangles[8].v1 = vec3( 5.0, 5.0, 5.0);
	triangles[8].v2 = vec3( 5.0, 5.0,-5.0);
	triangles[8].v3 = vec3(-5.0, 5.0, 5.0);
	triangles[8].MaterialIdx = 4;	
	triangles[9].v1 = vec3(-5.0, 5.0,-5.0);
	triangles[9].v2 = vec3(-5.0, 5.0, 5.0);
	triangles[9].v3 = vec3( 5.0, 5.0,-5.0);
	triangles[9].MaterialIdx = 4;
		
	// sphere1
	spheres[0].Center = vec3(-2.0,-2.0,-2.0);
	spheres[0].Radius = 2.0;
	spheres[0].MaterialIdx = 5;

	// sphere2
	spheres[1].Center = vec3(3.0,1.0,2.0);
	spheres[1].Radius = 1.0;
	spheres[1].MaterialIdx = 0;	
}

bool IntersectSphere( SSphere sphere, Ray ray, float start, float final, out float time ) 
{
	ray.Origin -= sphere.Center;
	float A = dot(ray.Direction, ray.Direction);
	float B = dot(ray.Direction, ray.Origin);
	float C = dot(ray.Origin, ray.Origin) - sphere.Radius * sphere.Radius;
	float D = B * B - A * C;	
	if (D >= 0.0) 
	{
		D = sqrt(D);
		float t1 = (-B - D) / A;
		float t2 = (-B + D) / A;		
		if(t1 >= 0 || t2 >= 0)
		{
			if(min(t1, t2) < 0)
			{
				time = max(t1, t2);
			}
			else
			{
				time = min(t1, t2);
			}			
			return true;
		}
	}	
	return false;
}

bool IntersectTriangle(Ray ray, vec3 v1, vec3 v2, vec3 v3, out float time) 
{
	time = -1;	
	vec3 A = v2 - v1; 
	vec3 B = v3 - v1; 
	vec3 N = cross(A, B);
	float NdotRayDirection = dot(N, ray.Direction);	
	if (abs(NdotRayDirection) < EPSILON) 
	{
		return false;
	}	
	float d = dot(N, v1); 
	float t = -(dot(N, ray.Origin) - d) / NdotRayDirection;	
	if (t < 0.0) 
	{
		return false;
	}	
	vec3 P = ray.Origin + t * ray.Direction;
	vec3 C;	
	vec3 edge1 = v2 - v1;
	vec3 VP1 = P - v1; 
	C = cross(edge1, VP1);
	if (dot(N, C) < 0.0) 
	{
		return false;
	}
	vec3 edge2 = v3 - v2;
	vec3 VP2 = P - v2;
	C = cross(edge2, VP2);
	if (dot(N, C) < 0.0) 
	{
		return false;
	}	
	vec3 edge3 = v1 - v3; 
	vec3 VP3 = P - v3;
	C = cross(edge3, VP3);
	if (dot(N, C) < 0.0) 
	{
		return false;
	}	
	time = t;
	return true;
}

struct SIntersection 
{
	float Time;
	vec3  Point;
	vec3  Normal;
	vec3  Color;
	vec4  LightCoeffs;	
	float ReflectionCoef;
	float RefractionCoef;	
	int   MaterialType;
};

bool Raytrace(Ray ray, float start, float final, inout SIntersection intersect )
{
	bool  result = false;
	float test   = start;	
	int		  MaterialIdx;
	STriangle triangle;
	SSphere	  sphere;	
	intersect.Time = final;	
	for (int i = 0; i < SPHERES_COUNT; i++) 
	{
		sphere 		= spheres[i];
		MaterialIdx = sphere.MaterialIdx;		
		if (IntersectSphere(sphere, ray, start, final, test) && test < intersect.Time) 
		{
			intersect.Time 	 = test;
			intersect.Point  = ray.Origin + ray.Direction * test;
			intersect.Normal = normalize(intersect.Point - spheres[i].Center);			
			intersect.Color  		 = materials[MaterialIdx].Color;
			intersect.LightCoeffs 	 = materials[MaterialIdx].LightCoeffs;
			intersect.ReflectionCoef = materials[MaterialIdx].ReflectionCoef;
			intersect.RefractionCoef = materials[MaterialIdx].RefractionCoef;
			intersect.MaterialType	 = materials[MaterialIdx].MaterialType;			
			result = true;
		}
	}	
	for (int i = 0; i < TRIANGLES_COUNT; i++) 
	{
		triangle 	= triangles[i];
		MaterialIdx = triangle.MaterialIdx;		
		if (IntersectTriangle(ray, triangle.v1, triangle.v2, triangle.v3, test) && test < intersect.Time ) 
		{
			intersect.Time   = test;
			intersect.Point  = ray.Origin + ray.Direction * test;
			intersect.Normal = normalize(cross(triangle.v1 - triangle.v2, triangle.v3 - triangle.v2));			
			intersect.Color  		 = materials[MaterialIdx].Color;
			intersect.LightCoeffs    = materials[MaterialIdx].LightCoeffs;
			intersect.ReflectionCoef = materials[MaterialIdx].ReflectionCoef;
			intersect.RefractionCoef = materials[MaterialIdx].RefractionCoef;
			intersect.MaterialType	 = materials[MaterialIdx].MaterialType;			
			result = true;
		}
	}	
	const int CUBE_CURRENT_TOTAL_TRIANGLES_COUNT = min(CUBE_TOTAL_TRIANGLES_COUNT, CubeLoadedCount * CUBE_TRIANGLES_COUNT);
	for (int i = 0; i < CUBE_CURRENT_TOTAL_TRIANGLES_COUNT; i++) 
	{
		triangle 	= CubeTriangles[i];
		MaterialIdx = triangle.MaterialIdx;		
		if (IntersectTriangle(ray, triangle.v1, triangle.v2, triangle.v3, test) && test < intersect.Time ) 
		{
			intersect.Time   = test;
			intersect.Point  = ray.Origin + ray.Direction * test;
			intersect.Normal = normalize(cross(triangle.v1 - triangle.v2, triangle.v3 - triangle.v2));			
			intersect.Color  		 = materials[MaterialIdx].Color;
			intersect.LightCoeffs    = materials[MaterialIdx].LightCoeffs;
			intersect.ReflectionCoef = materials[MaterialIdx].ReflectionCoef;
			intersect.RefractionCoef = materials[MaterialIdx].RefractionCoef;
			intersect.MaterialType	 = materials[MaterialIdx].MaterialType;			
			result = true;
		}
	}		
	return result;
}

vec3 Phong(SIntersection intersect, SLight currLight, float shadow) 
{
	vec3  light     = normalize(currLight.Position - intersect.Point);
	vec3  view 	    = normalize(uCamera.Position   - intersect.Point);
	float diffuse   = max(dot(light, intersect.Normal), 0.0);	
	vec3  reflected = reflect(-view, intersect.Normal);	
	float specular = pow(max(dot(reflected, light), 0.0), intersect.LightCoeffs.w);	
	return intersect.LightCoeffs.x * intersect.Color +
		   intersect.LightCoeffs.y * diffuse * intersect.Color * shadow +
		   intersect.LightCoeffs.z * specular * Unit;
}

float Shadow(SLight currLight, SIntersection intersect) 
{
	vec3  dir = normalize(currLight.Position - intersect.Point);
	float dis = distance(currLight.Position, intersect.Point);
	Ray shadowRay = Ray(intersect.Point + dir * EPSILON, dir);
	SIntersection shadowIntersect;
	shadowIntersect.Time = BIG;
	return int(!Raytrace(shadowRay, 0, dis, shadowIntersect));
}

Ray GenerateRay() 
{
	vec2 crd = glPosition.xy * uCamera.Scale;
	vec3 dir = uCamera.View + uCamera.Side * crd.x + uCamera.Up * crd.y;	
	return Ray(uCamera.Position, normalize(dir));
}

bool pushRay (STracingRay newray) 
{
	bool canBePlaced = (StackPTR < STACK_LENGTH);	
	if(canBePlaced) 
	{
		StackPTR += 1;
		Stack[StackPTR] = newray;
	}	
	return canBePlaced;
} 

STracingRay popRay () 
{
	StackPTR -= int(StackPTR >= 0);
	return Stack[StackPTR + 1];
}

bool isEmpty() 
{
	return !(StackPTR >= 0);
}

void main () 
{
	initializeDefaultScene(triangles, spheres);
	initializeDefaultLightMaterials(light, materials);	
	float start = 0;
	float final = BIG;
	vec3  resultColor = vec3(0,0,0);
	Ray ray = GenerateRay();	
	
	STracingRay sray = STracingRay(ray, 1, 0);
	pushRay(sray);	
	while (!isEmpty()) 
	{
		SIntersection intersect;
		STracingRay sray = popRay();
		intersect.Time = BIG;
		ray = sray.ray;		
		start = 0;
		final = BIG;		
		if (Raytrace(ray, start, final, intersect)) 
		{
			switch (intersect.MaterialType) 
			{
				case DIFFUSE_REFLECTION: 
				{				
					float shadow = Shadow(light, intersect);
					resultColor += sray.contribution * Phong(intersect, light, shadow);
					break;					
				}				
				case MIRROR_REFLECTION: 
				{				
					if (intersect.ReflectionCoef < 1) 
					{
						float shadow = Shadow(light, intersect);
						float contribution = sray.contribution * (1 - intersect.ReflectionCoef);						
						resultColor += contribution * Phong(intersect, light, shadow);
					}					
					vec3 reflectDirection = reflect(ray.Direction, intersect.Normal);					
					float contribution = sray.contribution * intersect.ReflectionCoef;
					if(sray.depth + 1 < RayTracingDepth) 
					{
						STracingRay reflectRay = STracingRay(Ray(intersect.Point + reflectDirection * EPSILON, reflectDirection), contribution, sray.depth + 1);
						pushRay(reflectRay);
					}
					break;					
				}
			}
		}
	}	
	FragColor = vec4 (resultColor, 1.0);
}