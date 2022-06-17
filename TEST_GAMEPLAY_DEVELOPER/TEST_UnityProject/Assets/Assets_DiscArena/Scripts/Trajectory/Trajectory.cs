using UnityEngine;
using UnityEngine.SceneManagement;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private Transform m_SceneParent = null;
    [SerializeField] private LineRenderer m_Line = null;
    [SerializeField] private int m_MaxPhysicsFrameIterations = 0;
    [SerializeField] private Disc m_FakeDiscPrefab = null;
    [SerializeField] private InputReader m_InputReader = null;
    private Scene m_SimulationScene = default;
    private PhysicsScene m_PhysicsSimulationScene = default;
    private Disc m_FakeDisc = null;

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
        }
        else
        {
            m_InputReader.OnTouchScreenEvent -= OnTouchScreen;
        }
    }

    private void OnTouchScreen(Vector2 touchPosition)
    {
        SimulateTrajectory(transform.position, touchPosition);
    }

    private void CreateSimulationScene()
    {
        m_SimulationScene = SceneManager.CreateScene("SimulationScene", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        m_PhysicsSimulationScene = m_SimulationScene.GetPhysicsScene();

        foreach (Transform sceneObject in m_SceneParent)
        {
            GameObject fakeObject = Instantiate(sceneObject.gameObject, sceneObject.position, sceneObject.rotation);
            SceneManager.MoveGameObjectToScene(fakeObject, m_SimulationScene);

            Renderer renderer = fakeObject.GetComponent<Renderer>();
            if (renderer)
            {
                renderer.enabled = false;
            }
        }

        m_Line.positionCount = m_MaxPhysicsFrameIterations;
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

    public void SimulateTrajectory(in Vector3 startPosition, in Vector2 mousePosition)
    {
        if (!m_FakeDisc)
        {
            return;
        }

        m_FakeDisc.transform.position = startPosition;
        m_FakeDisc.ShootDisc(mousePosition);

        const float lineOffsetY = 0.1f;

        for (int i = 0; i < m_MaxPhysicsFrameIterations; ++i)
        {
            m_PhysicsSimulationScene.Simulate(Time.fixedDeltaTime);
            Vector3 simulatedPosition = new Vector3(m_FakeDisc.transform.position.x, m_FakeDisc.transform.position.y + lineOffsetY, m_FakeDisc.transform.position.z);
            m_Line.SetPosition(i, simulatedPosition);
        }
    }
}
