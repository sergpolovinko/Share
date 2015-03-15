using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace benchmarking
{
    public struct Vector3f
    {
        public float x;
        public float y;
        public float z;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public Vector3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    };

    public class Point
    {
        public Vector3f P;
    }

    /// <summary>
    /// Compare performance of passing parameters by value/reference into simple math function.
    /// Compile in Release mode to see difference in performance.
    /// </summary>
    class Program
    {
        static DateTime _start;

        [MethodImpl(MethodImplOptions.NoInlining)]
        static Single Dot(Vector3f a, Vector3f b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static Single DotRef(ref Vector3f a, ref Vector3f b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        static void Main(string[] args)
        {
            const long N = 50000000;
            var a = new Vector3f(100000, 100000, 100000);
            var XAxis = new Vector3f(1, 0, 0);

            float dummy = 0;

            var points = new Point[N];
            for (long i = 0; i < N; i++)
            {
                points[i] = new Point();
                points[i].P = new Vector3f((float)i, (float)i, (float)i);
            }

            // Passing parameter by value
            _start = DateTime.Now;

            for (long i = 0; i < N; i++)
                dummy += Dot(a, XAxis);

            Console.WriteLine("Local variable by value      {0}", DateTime.Now - _start);

            // Passing parameter by reference
            _start = DateTime.Now;

            for (long i = 0; i < N; i++)
                dummy += DotRef(ref a, ref XAxis);

            Console.WriteLine("Local variable by reference  {0}", DateTime.Now - _start);

            // Passing field of object as parameter by value with unboxing
            _start = DateTime.Now;

            for (long i = 0; i < N; i++)
                dummy += Dot(points[i].P, XAxis);

            Console.WriteLine("Object field by value        {0}", DateTime.Now - _start);


            // Passing field of object as parameter by reference to avoid unboxing
            _start = DateTime.Now;

            for (long i = 0; i < N; i++)
                dummy += DotRef(ref points[i].P, ref XAxis);

            Console.WriteLine("Object field by reference    {0}", DateTime.Now - _start);
        }
    }
}
