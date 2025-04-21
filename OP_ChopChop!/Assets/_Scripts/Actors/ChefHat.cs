using System.Collections;

public class ChefHat : PersistentSingleton<ChefHat> {

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit(); 

    public void StartService() => StartCoroutine(PreService());

    private IEnumerator PreService()
    {
        StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));
        yield return null;

        GameManager.Instance.ChangeShift(GameShift.PRE_SERVICE);
    }
}
