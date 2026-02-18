using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;

public class CharacterSwitch : MonoBehaviour
{
    private PlayerInput playerInput;
    private KaiController kaiController;
    private OllieController ollieController;
    private BaileyController baileyController;
    private SpriteLibrary spriteLibrary;
    private SpriteResolver spriteResolver;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        kaiController = GetComponent<KaiController>();
        ollieController = GetComponent<OllieController>();
        baileyController = GetComponent<BaileyController>();

        spriteLibrary = GetComponent<SpriteLibrary>();
        spriteResolver = GetComponent<SpriteResolver>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KaiSwitch()
    {
        var currentResolverCategory = spriteResolver.GetCategory();
        if (kaiController.enabled)
        return;

        else
        {
        playerInput.SwitchCurrentActionMap("Kai");
        playerInput.defaultActionMap = "Kai";

        spriteLibrary.spriteLibraryAsset = (SpriteLibraryAsset)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Characters/SpriteLibraries/Kai.spriteLib", typeof (SpriteLibraryAsset));
        spriteResolver.SetCategoryAndLabel("Default", "spritesheet_0");

        kaiController.enabled = true;
        if (ollieController.enabled)
        ollieController.enabled = false;

        if(baileyController.enabled)
        baileyController.enabled = false;
        }
    }

    public void OllieSwitch()
    {
        var currentResolverCategory = spriteResolver.GetCategory();
        if (ollieController.enabled)
        {
            Debug.LogError("You're already Ollie!");
            return;
        }
        /*if (!GameManager.instance.OllieSaved)
        {
            Debug.LogError("You haven't saved Ollie yet!");
            return;
        }
        */

        else
        {
        playerInput.SwitchCurrentActionMap("Ollie");
        playerInput.defaultActionMap = "Ollie";

        spriteLibrary.spriteLibraryAsset = (SpriteLibraryAsset)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Characters/SpriteLibraries/Ollie.spriteLib", typeof (SpriteLibraryAsset));
        spriteResolver.SetCategoryAndLabel("Default", "spritesheet_0");

        ollieController.enabled = true;
        if (kaiController.enabled)
        kaiController.enabled = false;

        if(baileyController.enabled)
        baileyController.enabled = false;
        }
    }

    public void BaileySwitch()
    {
        var currentResolverCategory = spriteResolver.GetCategory();
        if (baileyController.enabled)
        return;

        /*if (!GameManager.instance.BaileySaved)
        {
            Debug.LogError("You haven't saved Bailey yet!");
            return;
        }
        */

        else
        {
        playerInput.SwitchCurrentActionMap("Bailey");
        playerInput.defaultActionMap = "Bailey";

        spriteLibrary.spriteLibraryAsset = (SpriteLibraryAsset)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Characters/SpriteLibraries/Bailey.spriteLib", typeof (SpriteLibraryAsset));
        spriteResolver.SetCategoryAndLabel("Default", "spritesheet_0");

        baileyController.enabled = true;
        if (kaiController.enabled)
        kaiController.enabled = false;

        if(ollieController.enabled)
        ollieController.enabled = false;
        }
    }
}
