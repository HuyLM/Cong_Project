using UnityEngine;

namespace AtoLib {
    #region Normal class - NONE MonoBehaviour
    /**<summary> Singleton class which is not MonoBehavior</summary> */
    public class Singleton<T> where T : new() {
        private static T instance;

        /**<summary> If you want to check null, use 'if (T.Initialized)' instead of 'if (T.Instance != null)' </summary> */
        public static bool Initialized { get; protected set; }

        public static T Instance {
            get {
                if (instance != null)
                    return instance;
                instance = new T();
                (instance as Singleton<T>).Initialize();
                Initialized = true;
                return instance;
            }
        }

        protected virtual void Initialize() {
            if (Initialized)
                return;
        }

        /**<summary> Call T.Instance.Preload() at the first script startup to pre init. </summary>*/
        public virtual void Preload() { }
    }

    #endregion

    #region MonoBehaviour
    /** <summary> Base Singleton class which is MonoBehavior </summary> */
    public abstract class SingletonMono<T> : MonoBehaviour where T : Component {
        protected static T instance;

        /**<summary> If you want to check null, use 'if (T.Initialized)' instead of 'if (T.Instance != null)' </summary> */
        public static bool Initialized {
            get { return instance != null; }
        }

        protected virtual void Awake() {
            if (instance == null)
                instance = this as T;
            else if (this != instance) {
                Debug.LogWarningFormat("[MonoSingleton] Class {0} is initialized multiple times", typeof(T).FullName);
                DestroyImmediate(gameObject);
                return;
            }

            OnAwake();
        }

        protected abstract void OnAwake();

        /**<summary> Call T.Instance.Preload() at the first script startup to pre init. </summary>*/
        public virtual void Preload() { }

        protected virtual void OnDestroy() {
            instance = null;
        }
    }

    /** <summary> 
     * <para> "Instance" = Find object in scene. </para>
     * <para> Must be added to scene before run </para>
     * </summary> */
    public class SingletonBind<T> : SingletonMono<T> where T : Component {
        /**<summary> If you want to check null, use 'if (T.Initialized)' instead of 'if (T.Instance != null)' </summary> */
        public static T Instance {
            get {
                if (instance != null)
                    return instance;
                instance = FindObjectOfType<T>();
                if (instance == null) {
                    Debug.LogErrorFormat("[Singleton] Class {0} must be added to scene before run!", typeof(T));
                }
                return instance;
            }
        }

        protected override void OnAwake() { }
    }

    /** <summary> 
     * <para> "Instance" = Find object in scene. </para>
     * <para> Must be added to scene before run </para>
     * <para> Instance is DontDestroyOnLoad </para>
     * </summary> */
    public class SingletonBindAlive<T> : SingletonBind<T> where T : Component {
        protected override void Awake() {
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }
    }

    /** <summary> 
     * <para>"Instance" = new GameObject if can not find it on scene. </para>
     * <para> No scene reference variables. </para>
     * </summary> */
    public class SingletonFree<T> : SingletonMono<T> where T : Component {
        /**<summary> If you want to check null, use 'if (T.Initialized)' instead of 'if (T.Instance != null)' </summary> */
        public static T Instance {
            get {
                if (instance != null)
                    return instance;
                instance = FindObjectOfType<T>();
                if (instance == null) {
                    Debug.LogFormat("[Singleton] Class {0} not found! Create empty instance", typeof(T));
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
                return instance;
            }
        }

        protected override void OnAwake() { }
    }

    /** <summary> 
     * <para> "Instance" = new GameObject if can not find it on scene. </para>
     * <para> No scene reference variables. </para>
     * <para> Instance is DontDestroyOnLoad </para>
     * </summary> */
    public class SingletonFreeAlive<T> : SingletonFree<T> where T : Component {
        protected override void Awake() {
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }

        public static bool HasInstance => instance != null;
    }

    /** <summary> 
     * <para> "Instance" = Instantiate from Resources folder when be called at runtime.</para>
     * <para> Place your prefab in Resources: "Prefabs/T/T", T is the name of class </para> 
     * </summary> */
    public class SingletonResource<T> : SingletonMono<T> where T : Component {
        protected static string ResourcePath {
            get => string.Format("Prefabs/{0}/{1}", typeof(T).Name, typeof(T).Name);
        }

        /**<summary> If you want to check null, use 'if (T.Initialized)' instead of 'if (T.Instance != null)' </summary> */
        public static T Instance {
            get {
                if (instance != null)
                    return instance;
                var g = Resources.Load<GameObject>(ResourcePath);
                if (g == null) {
                    Debug.LogErrorFormat("[{0}] Wrong resources path: {1}!", typeof(T).Name, ResourcePath);
                    return instance;
                }

                instance = Instantiate(g).GetComponent<T>();
                if (instance == null) {
                    Debug.LogErrorFormat("[{0}] Component not found in object: {1}!", typeof(T).Name, ResourcePath);
                }

                return instance;
            }
        }

        protected override void OnAwake() { }
    }

    /** <summary>
     * <para> "Instance" = Instantiate from Resources folder when be called at runtime. </para>
     * <para> Place your prefab in Resources: "Prefabs/T/T", T is the name of class </para> 
     * <para> Instance is DontDestroyOnLoad</para>
     * </summary> */
    public class SingletonResourceAlive<T> : SingletonResource<T> where T : Component {
        protected override void Awake() {
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }
    }
    #endregion
}