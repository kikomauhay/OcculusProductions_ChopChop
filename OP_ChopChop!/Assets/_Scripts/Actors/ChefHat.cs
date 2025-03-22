using System.Collections;

public class ChefHat : StaticInstance<ChefHat> {

    public bool HatWorn { get; private set; } = false;

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit(); 

    public void StartService() 
    {
        HatWorn = true;
        StartCoroutine(PreService());
    }

    IEnumerator PreService()
    {
        SceneHandler.Instance.GoToScene("MainGameScene");
        yield return null;

        GameManager.Instance.ChangeShift(GameShift.PRE_SERVICE);
    }
}
