using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.Vectors {

    public static class Vector3Extensions {

        /* quick vector generators for float values */
        public static Vector3 X(this float x) => new Vector3( x, 0, 0);    
        public static Vector3 Y(this float y) => new Vector3( 0, y, 0);    
        public static Vector3 Z(this float z) => new Vector3( 0, 0, z);    

        /* quick vector generators for int values */
        public static Vector3 X(this int x) => new Vector3( x, 0, 0);    
        public static Vector3 Y(this int y) => new Vector3( 0, y, 0);    
        public static Vector3 Z(this int z) => new Vector3( 0, 0, z);    
        
        /* axial projections */
        public static Vector3 X(this Vector3 v) => new Vector3(v.x,   0,   0);    
        public static Vector3 Y(this Vector3 v) => new Vector3(  0, v.y,   0);    
        public static Vector3 Z(this Vector3 v) => new Vector3(  0,   0, v.z);    

        /* planar projections */  
        public static Vector3 YZ(this Vector3 v) => new Vector3(  0, v.y, v.z);
        public static Vector3 XZ(this Vector3 v) => new Vector3(v.x,   0, v.z);
        public static Vector3 XY(this Vector3 v) => new Vector3(v.x, v.y,   0);

        /* planar projections */  
        public static Vector3 YZ(this float v) => new Vector3(0, v, v);
        public static Vector3 XZ(this float v) => new Vector3(v, 0, v);
        public static Vector3 XY(this float v) => new Vector3(v, v, 0);
        
        /* planar projections */  
        public static Vector3 YZ(this int v) => new Vector3(0, v, v);
        public static Vector3 XZ(this int v) => new Vector3(v, 0, v);
        public static Vector3 XY(this int v) => new Vector3(v, v, 0);

        /* spatial projections */
        public static Vector3 XYZ(this float v) => new Vector3(v, v, v);
        public static Vector3 XYZ(this int v) => new Vector3(v, v, v);
        public static Vector3 XYZ(this (float, float, float) t) => new Vector3(t.Item1, t.Item2, t.Item3);


        /* map functions over each axis */
        public static Vector3 Map(this Vector3 v, Func<float, float> f, Func<float, float> g, Func<float, float> h) {
            return new Vector3( f(v.x), g(v.y), h(v.z) );
        }
        
        /* map single function over all the vector axes */
        public static Vector3 Map(this Vector3 v, Func<float, float> f) => v.Map(f,f,f);

        /* elementwise multiplication, same as Vector3.Scale but not static */
        public static Vector3 Multiply(this Vector3 v, Vector3 s) {
            return Vector3.Scale(v,s);
        }

        /* overloaded method with unpacked arguments */
        public static Vector3 Multiply(this Vector3 v, float x, float y, float z) {
            return Vector3.Scale(v, new Vector3(x,y,z));
        }

        /* elementwise division*/
        public static Vector3 Divide(this Vector3 v, Vector3 s) {
            return new Vector3(v.x/s.x,v.y/s.y,v.z/s.z);
        }

        public static Vector3 Divide(this Vector3 v, float x, float y, float z) {
            return new Vector3(v.x/x,v.y/y,v.z/z);
        }

        /* get the shortest axis */
        public static float Minimum(this Vector3 v) {
            return Mathf.Min(v.x, v.y, v.z);
        }

        /* get the longest axis */
        public static float Maximum(this Vector3 v) {
            return Mathf.Max(v.x, v.y, v.z);
        }
        
        /* vector version of mathf smoothstep function */
        public static Vector3 SmoothStep(Vector3 from, Vector3 to, float step) {
            return from.Map(
                (float x) => Mathf.SmoothStep(x, to.x, step),
                (float y) => Mathf.SmoothStep(y, to.y, step),
                (float z) => Mathf.SmoothStep(z, to.z, step)
            );
        }

        public static Vector2 V2D(this Vector3 v) {
            return new Vector2(v.x, v.z);
        }
        
    }

    public static class Vector2Extensions {
        
        public static Vector2 X(this Vector2 v) => new Vector2(v.x,   0);    
        public static Vector2 Y(this Vector2 v) => new Vector2(  0, v.y);

        public static Vector3 XY(this Vector2 v) => new Vector3(v.x, v.y,   0);
        public static Vector3 YZ(this Vector2 v) => new Vector3(  0, v.y, v.x);
        public static Vector3 XZ(this Vector2 v) => new Vector3(v.x,   0, v.y);

        public static Vector2 Map(this Vector2 v, Func<float, float> f, Func<float, float> g) {
            return new Vector2( f(v.x), g(v.y) );
        }

        public static Vector2 Map(this Vector2 v, Func<float, float> f) => v.Map(f,f);

        public static Vector2 Multiply(this Vector2 v, Vector2 s) {
            return Vector2.Scale(v,s);
        }

        public static Vector2 Multiply(this Vector2 v, float x, float y) {
            return Vector2.Scale(v, new Vector3(x,y));
        }

        public static Vector2 Divide(this Vector2 v, Vector2 s) {
            return new Vector2(v.x/s.x,v.y/s.y);
        }

        public static Vector2 Divide(this Vector2 v, float x, float y) {
            return new Vector2(v.x/x,v.y/y);
        }


        public static float Minimum(this Vector2 v) {
            return Mathf.Min(v.x, v.y);
        }

        public static float Maximum(this Vector2 v) {
            return Mathf.Max(v.x, v.y);
        }

        public static Vector3 V3D(this Vector2 v, float yPos = 0) {
            return new Vector3(v.x, yPos, v.y);
        }

    }

}

namespace Utility.Math {

    public enum Axis { X = 0, Y = 1, Z = 2}
    
    public static class MathExtensions {

        /* Bias behaves the same with Mathf.Sign except
         * that if the input is zero it retuns zero.
         */
        public static float Bias(this float n, float epsilon = 0.01f) {
            return Mathf.Abs(n) <= epsilon ? 0 : (n > 0 ? 1 : -1);
        }

        public static Quaternion Clamp(this Quaternion q, float min, float max, Axis axis) {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;
        
            int idx = (int)axis;
            float angle = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q[idx]);
            angle = Mathf.Clamp(angle, min, max);
            q[idx] = Mathf.Tan(0.5f * Mathf.Deg2Rad * angle);
        
            return q;
        }

        public static int Rounded(this float n) {
            return Mathf.RoundToInt(n);
        }
    }

    [System.Serializable]
    public struct Range {
        
        public float min, max;

        public static Range zero { 
            get { return new Range(0,0); } 
        }

        public Range(float n, float x) {
            min = n;
            max = Mathf.Max(x,min);
        }

        public float span {
            get { return max-min; }
        }

    }

}

namespace Utility.Random {

    public static class RandomExtensions {

        /* Randomly picks an element from an array */
        public static T RandomPick<T>(this T[] choices) {
            return choices.Length > 0 ?
                choices[UnityEngine.Random.Range(0, choices.Length)]
            :   default(T);
        }

        /* Randomly picks an element from an array with given frequencies */
        public static T RandomPick<T>(this T[] choices, float[] frequencies) {

            if(frequencies.Length != choices.Length) throw new Exception("item/frequency numbers don't match");
            
            float total = frequencies.Sum();
            float fidx = UnityEngine.Random.Range(0, total);

            float ct = 0;
            int i = 0;
            foreach (float freq in frequencies) {
                if (fidx <= ct) return choices[i-1];
                ct += freq;
                i  += 1;
            }
            return choices[i-1];
        }

        /* Randomly picks an element from a list */
        public static T RandomPick<T>(this List<T> choices) {
            return choices.Count > 0 ?
                choices[UnityEngine.Random.Range(0, choices.Count)]
            :   default(T);
        }

    }
}

namespace Utility.Colors {

    public static class ColorExtensions {
        
        public static Color R(this Color c, float r) => new Color(  r, c.g, c.b, c.a);
        public static Color G(this Color c, float g) => new Color(c.r,   g, c.b, c.a);
        public static Color B(this Color c, float b) => new Color(c.r, c.g,   b, c.a);
        public static Color A(this Color c, float a) => new Color(c.r, c.g, c.b,   a);


        public static float H(this Color c) {
            float h,s,v;
            Color.RGBToHSV(c, out h, out s, out v);
            return h;
        }

        public static float S(this Color c) {
            float h,s,v;
            Color.RGBToHSV(c, out h, out s, out v);
            return s;
        }

        public static float V(this Color c) {
            float h,s,v;
            Color.RGBToHSV(c, out h, out s, out v);
            return v;
        }

        public static Color H(this Color c, float l) {
            float h,s,v;
            Color.RGBToHSV(c, out h, out s, out v);
            return Color.HSVToRGB(l,s,v);
        }

        public static Color S(this Color c, float l) {
            float h,s,v;
            Color.RGBToHSV(c, out h, out s, out v);
            return Color.HSVToRGB(h,l,v);
        }

        public static Color V(this Color c, float l) {
            float h,s,v;
            Color.RGBToHSV(c, out h, out s, out v);
            return Color.HSVToRGB(h,s,l);
        }
    }

}

namespace Utility.Collections {

    public static class ListExtensions {

        public static void Enumerate<T>(this IEnumerable<T> ie, Action<int, T> action) {
            int i = 0;
            foreach (var e in ie) action(i++, e);
        }

        public static IEnumerable<R> Map<T, R>(this IEnumerable<T> self, Func<T, R> selector) {
            return self.Select(selector);
        }

        public static T Reduce<T>(this IEnumerable<T> self, Func<T, T, T> func) {
            return self.Aggregate(func);
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> self, Func<T, bool> predicate) {
            return self.Where(predicate);
        }

    }

    public static class ArrayExtensions {

        public static void Each<T>(this T[] elems, Action<T> f) {
            for(int i=0; i<elems.Length; i++) {
                f(elems[i]);
            }
        }

    }

}

namespace Utility.Scheduling {

    public delegate void Trigger();

    public class Timer {

        private float   _span;
        private float   _tick;
        private bool   _loops;
        private bool   _active;
        private Trigger _trigger;

        public void Add(Trigger f, float t, bool loops = false) {
            _trigger += f;
            _span = t;
            _tick = t;
            _loops = loops;
            _active = true;
        }

        public void Tick(float dt) {
            if(!_active) return;
            _tick -= dt;
            if (_tick < 0) {
                _trigger();
                if(_loops) _tick = _span;
                else _active = false;
            }
        }

        public void Reset() {
            _active = true;
            _tick = _span;
        }

    }
    
    public struct Task {
        
        private MonoBehaviour runner;
        private Action action;

        public Task(MonoBehaviour r, Action a) {
            runner = r;
            action = a;
        }

        public void After(float delay) {
            if(!runner.isActiveAndEnabled) {
                Debug.LogWarning("InactiveScript: aborting coroutine.");
                return;
            }
            runner.StartCoroutine(IRun(delay, action));
        }

        public void Now() {
            After(0);
        }

        public void Snychronously() {
            action();
        }

        private IEnumerator IRun(float delay, Action t) {
            yield return new WaitForSeconds(delay); 
            t();
        }

    }

    public static class TaskExtension { 

        public static Task Schedule(this MonoBehaviour runner, Action action) {
            return new Task(runner, action);
        }

    }

}

namespace Utility.Channels {

    public struct Payload {
        readonly public MonoBehaviour script;
        readonly public string message;

        public Payload(MonoBehaviour s, string m = "") {
            script = s;
            message = m;
        } 
    }

    public delegate void Response(Payload? p);
    
    public class Channel {

        public static Channel main;
        private Dictionary<string, Response> events;
        private static void Nop(Payload? p) {}

        public static void Setup() { 
            if (main == null) main = new Channel();
            main.events = new Dictionary<string, Response>(); 
        }

        public static void Unsubscribe(string key, Response r) {
            if (main.events.ContainsKey(key)) main.events[key] -= r;
        }

        public static void Subscribe(string key, Response r) {
            if (!main.events.ContainsKey(key)) main.events.Add(key, Nop);
            main.events[key] += r;
        }

        public static void Broadcast(string key, Payload? p = null) {
            if (!main.events.ContainsKey(key)) return;
            main.events[key](p);
        }

        public static void Reset() { 
            main.events = new Dictionary<string, Response>(); 
        }
        
    }

}

namespace Utility.Experimental {
    
    namespace Functional {

        public static class F {

            public static T Id<T>(T self) => self;

            public static Func<T1,T3> Compose<T1,T2,T3>(this Func<T2,T3> f, Func<T1, T2> g) {
                return (T1 x) => f(g(x));
            }

        }

    }

    public static class FlowExtensions {
        public static void Times(this int count, Action f) {
            for(int i=0; i<count; i++) f();
        }
    }

}
