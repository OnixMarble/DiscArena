using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private Transform m_SceneParent = null;
    [SerializeField] private DiscProjectile m_FakeDiscPrefab = null;
    [SerializeField] private InputReader m_InputReader = null;
    [SerializeField] private GameEvents m_GameEvents = null;
    [SerializeField] private int m_MaxPhysicsFrameIterations = 0;
    private LineRenderer m_Line = null;
    private Scene m_SimulationScene = default;
    private PhysicsScene m_PhysicsSimulationScene = default;
    private DiscProjectile m_FakeDisc = null;
    private Dictionary<GameObject, GameObject> m_ObjectMapping = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        m_Line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        CreateSimulationScene();
        CreateSimulationSubject();
    }

    private void OnEnable()
    {
        SetupCallbacks(true);
    }

    private void OnDisable()
    {
        SetupCallbacks(false);
    }

    private void SetupCallbacks(bool bind)
    {
        if (bind)
        {
            m_InputReader.OnTouchScreenEvent += OnTouchScreen;
            m_InputReader.OnShootEvent += OnShoot;
            m_GameEvents.OnNewTurnEvent += OnNewTurn;
        }
        else
        {
            m_InputReader.OnTouchScreenEvent -= OnTouchScreen;
            m_InputReader.OnShootEvent -= OnShoot;
            m_GameEvents.OnNewTurnEvent -= OnNewTurn;
        }
    }

    private void OnShoot(Vector2 touchPosition)
    {
        // Reset line renderer when disc has been shot
        m_Line.positionCount = 0;
    }

    private void OnTouchScreen(Vector2 touchPosition)
    {
        SimulateTrajectory(transform.position, touchPosition);
    }

    private void CreateSimulationScene()
    {
        m_SimulationScene = SceneManager.CreateScene("SimulationScene", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        m_PhysicsSimulationScene = m_SimulationScene.GetPhysicsScene();

        // Create root object for simulation scene
        GameObject simulationAreaRoot = new GameObject("SimulationAreaRoot");
        simulationAreaRoot.transform.localScale = new Vector3(0.5f, 1.0f, 1.0f);

        foreach (Transform sceneObject in m_SceneParent)
        {
            // Create the physics objects and attach them to root object
            GameObject fakeObject = Instantiate(sceneObject.gameObject, sceneObject.position, sceneObject.rotation, simulationAreaRoot.transform);
            fakeObject.tag = "Simulation";
            HideGraphics(fakeObject);

            if (sceneObject.gameObject.isStatic)
            {
                continue;
            }

            m_ObjectMapping.Add(sceneObject.gameObject, fakeObject);
        }

        SceneManager.MoveGameObjectToScene(simulationAreaRoot, m_SimulationScene);
    }

    private void CreateSimulationSubject()
    {
        if (m_FakeDisc)
        {
            return;
        }

        m_FakeDisc = Instantiate(m_FakeDiscPrefab);
        m_FakeDisc.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        SceneManager.MoveGameObjectToScene(m_FakeDisc.gameObject, m_SimulationScene);

        Renderer renderer = m_FakeDisc.GetComponentInChildren<Renderer>();
        if (renderer)
        {
            renderer.enabled = false;
        }
    }

    private void HideGraphics(in GameObject fakeObject)
    {
        Renderer[] renderers = fakeObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }

        Image[] images = fakeObject.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            image.enabled = false;
        }
    }

    private void OnNewTurn()
    {
        // Update simulation world objects
        foreach (KeyValuePair<GameObject, GameObject> sceneObject in m_ObjectMapping)
        {
            GameObject mainObject = sceneObject.Key;
            if (mainObject == null)
            {
                Destroy(sceneObject.Value);
            }
        }
    }

    public void SimulateTrajectory(in Vector3 startPosition, in Vector2 mousePosition)
    {
        if (!m_FakeDisc)
        {
            return;
        }

        m_FakeDisc.transform.position = startPosition;
        m_FakeDisc.ShootDisc(mousePosition);

        const float lineOffsetY = 0.1f;
        m_Line.positionCount = m_MaxPhysicsFrameIterations;

        for (int i = 0; i < m_MaxPhysicsFrameIterations; ++i)
        {
            m_PhysicsSimulationScene.Simulate(Time.fixedDeltaTime);
            Vector3 simulatedPosition = new Vector3(m_FakeDisc.transform.position.x, m_FakeDisc.transform.position.y + lineOffsetY, m_FakeDisc.transform.position.z);
            m_Line.SetPosition(i, simulatedPosition);
        }
    }

}
