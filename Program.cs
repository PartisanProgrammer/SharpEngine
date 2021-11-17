﻿using System;
using GLFW;

namespace SharpEngine
{
    class Program {
        static float Lerp(float from, float to, float t) {
            return from + (to - from) * t;
        }

        static float GetRandomFloat(Random random, float min = 0, float max = 1) {
            return Lerp(min, max, (float)random.Next() / int.MaxValue);
        }
        
        static void Main(string[] args) {
            
            var window = new Window();
            var material = new Material("shaders/world-position-color.vert", "shaders/vertex-color.frag");
            var scene = new Scene();
            window.Load(scene);

            var shape = new Triangle(material);
            shape.Transform.CurrentScale = new Vector(0.5f, 1f, 1f);
            scene.Add(shape);
            
            var rectangle = new Rectangle(material);
            rectangle.Transform.CurrentScale = new Vector(0.8f, 3f, 1f);
            rectangle.Transform.Position = new Vector(0f, 0f);
            
            scene.Add(rectangle);

            
            var ground = new Rectangle(material);
            ground.Transform.CurrentScale = new Vector(10f, 1f, 1f);
            ground.Transform.Position = new Vector(0f, -1f);
            scene.Add(ground);

            // engine rendering loop
            const int fixedStepNumberPerSecond = 30;
            const float fixedDeltaTime = 1.0f / fixedStepNumberPerSecond;
            const float movementSpeed = 0.5f;
            double previousFixedStep = 0.0;
            
            
            while (window.IsOpen()) {
                while (Glfw.Time > previousFixedStep + fixedDeltaTime) {
                    previousFixedStep += fixedDeltaTime;
                    var walkDirection = new Vector();
                    var recDirection = rectangle.GetCenter() - shape.GetCenter();

                    
                    if (Vector.Dot(shape.Transform.Forward, recDirection) > 0 )
                    {
                        rectangle.SetColor(Color.Red);
                    }
                    else 
                    {
                        rectangle.SetColor(Color.Blue);
                    }

                    //Movement
                    if (window.GetKey(Keys.W))
                    {
                        walkDirection += shape.Transform.Forward;
                    }
                    if (window.GetKey(Keys.S))
                    {
                       // walkDirection += Vector.Backward;
                       walkDirection += shape.Transform.Backward;
                    }
                    if (window.GetKey(Keys.A))
                    {
                        walkDirection += shape.Transform.Left;
                    }
                    if (window.GetKey(Keys.D))
                    {
                        walkDirection += shape.Transform.Right;
                    }
                    
                    //Rotation
                    if (window.GetKey(Keys.E))
                    {
                        var rotation = shape.Transform.Rotation;
                        rotation.z -= (2*MathF.PI * fixedDeltaTime)/3;
                        shape.Transform.Rotation = rotation;
                    }
                    if (window.GetKey(Keys.Q))
                    {
                        var rotation = shape.Transform.Rotation;
                        rotation.z += (2*MathF.PI * fixedDeltaTime)/3;
                        shape.Transform.Rotation = rotation;
                    }

                    walkDirection = walkDirection.Normalize();
                    shape.Transform.Position += walkDirection * movementSpeed*fixedDeltaTime;
                }
                window.Render();
            }
        }
    }
}