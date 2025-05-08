using System.Collections;

public class ChefHat : PersistentSingleton<ChefHat> {

    public System.Action OnHatWorn;

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit(); 

    public void StartService() => StartCoroutine(PreService());

    private IEnumerator PreService()
    {
        StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));
        yield return null;

        GameManager.Instance.ChangeShift(GameShift.PreService);
        ShopManager.Instance.ClearList();
        OnHatWorn?.Invoke();
    }
}
