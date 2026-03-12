using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;

public class CharacterSwitch : MonoBehaviour
{
    private PlayerInput playerInput;
    /* private KaiController kaiController;
    private OllieController ollieController;
    private BaileyController baileyController; */
    private SpriteLibrary spriteLibrary;
    private SpriteResolver spriteResolver;
    private MainController mainController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        spriteLibrary = GetComponent<SpriteLibrary>();
        spriteResolver = GetComponent<SpriteResolver>();
        mainController = GetComponent<MainController>();
    }
    /* void Start()
    {
        
        playerInput = GetComponent<PlayerInput>();


        spriteLibrary = GetComponent<SpriteLibrary>();
        spriteResolver = GetComponent<SpriteResolver>();
        mainController = GetComponent<MainController>();
        
    }
    */

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KaiSwitch()
    {
        var currentResolverCategory = spriteResolver.GetCategory();
        /* if (kaiController.enabled)
        return;

        else
        { */
        playerInput.SwitchCurrentActionMap("Kai");
        playerInput.defaultActionMap = "Kai";

        spriteLibrary.spriteLibraryAsset = Resources.Load<SpriteLibraryAsset>("SpriteLibraries/Kai");
        spriteResolver.SetCategoryAndLabel("Default", "spritesheet_0");

        //mainController.jumpSound = Resources.Load<AudioClip>("Audio/jump-kai");


        /* kaiController.enabled = true;
        if (ollieController.enabled)
        ollieController.enabled = false;

        if(baileyController.enabled)
        baileyController.enabled = false;
        } */
    }

    public void OllieSwitch()
    {
        var currentResolverCategory = spriteResolver.GetCategory();
        /* if (ollieController.enabled)
        {
            Debug.LogError("You're already Ollie!");
            return;
        } */
        /*if (!GameManager.instance.OllieSaved)
        {
            Debug.LogError("You haven't saved Ollie yet!");
            return;
        }
        */

        /* else
        { */
        playerInput.SwitchCurrentActionMap("Ollie");
        playerInput.defaultActionMap = "Ollie";

        spriteLibrary.spriteLibraryAsset = Resources.Load<SpriteLibraryAsset>("SpriteLibraries/Ollie");
        spriteResolver.SetCategoryAndLabel("Default", "spritesheet_0");

        //mainController.jumpSound = Resources.Load<AudioClip>("Audio/jump-ollie");

        /* ollieController.enabled = true;
        if (kaiController.enabled)
        kaiController.enabled = false;

        if(baileyController.enabled)
        baileyController.enabled = false;
        } */
    }

    public void BaileySwitch()
    {
        /* var currentResolverCategory = spriteResolver.GetCategory();
        if (baileyController.enabled)
        return; */

        /*if (!GameManager.instance.BaileySaved)
        {
            Debug.LogError("You haven't saved Bailey yet!");
            return;
        }
        */

        /* else
        { */
        playerInput.SwitchCurrentActionMap("Bailey");
        playerInput.defaultActionMap = "Bailey";

        spriteLibrary.spriteLibraryAsset = Resources.Load<SpriteLibraryAsset>("SpriteLibraries/Bailey");
        spriteResolver.SetCategoryAndLabel("Default", "spritesheet_0");

        //mainController.jumpSound = Resources.Load<AudioClip>("Audio/jump-bailey");

        /* baileyController.enabled = true;
        if (kaiController.enabled)
        kaiController.enabled = false;

        if(ollieController.enabled)
        ollieController.enabled = false;
        } */
    }
}
