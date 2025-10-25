using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.AI.Navigation;
using System.Collections.Generic;
using System;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
 
    public PlayerInput PlayerInput;
    [SerializeField] ButtonController buttonController;

    public GameObject player;
    public playerController playerScript;


    public InputActionReference menuIn;
    public InputActionReference menuOut;

   public Image selectedBar;

    public bool isPaused = false;
     float timesScaleOrig;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        timesScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
       
    }


    public void statePause()
    {

        isPaused = !isPaused;


        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        PlayerInput.SwitchCurrentActionMap("Menus");
       

    }

    public void stateUnpause()
    {

        isPaused = !isPaused;

        Time.timeScale = timesScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
        PlayerInput.SwitchCurrentActionMap("Gameplay");


    }


    public void YouLose()
    {
      
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
        StartCoroutine(FadeMenus(1f, 0.3f, menuActive));
    }
    public void YouWin()
    {
       
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
        StartCoroutine(FadeMenus(1f, 0.3f, menuActive));
    }
    void Pause(InputAction.CallbackContext context)
    {
        if (menuActive == null)
        {

         
            statePause();
            menuActive = menuPause;
            menuActive.SetActive(true);



        }
        else if (menuActive == menuPause)
        {
            stateUnpause();

            buttonController.buttons.Clear();
        }

    }
 
  

    IEnumerator FadeMenus(float to, float duration, GameObject objectToFade)
    {
        CanvasGroup canvasGroup = objectToFade.GetComponent<CanvasGroup>();
        if (canvasGroup == null) yield break;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, to, timer / duration);
            yield return null;
        }
    }
    private void OnEnable()
    {
        menuIn.action.started += Pause;
        menuOut.action.started += Pause;
        

    }
    private void OnDisable()
    {
        menuIn.action.started -= Pause;
        menuOut.action.started -= Pause;

    }


    void Update()
    {

    }

    //public void FlashScreen(Color color)
    //{
    //    color.a = 0.15f;
    //   playerGetsDamaged.GetComponent<Image>().color = color;

    //    StartCoroutine(flashDamageScreen());
    //}

    //IEnumerator flashDamageScreen()
    //{
    //    playerGetsDamaged.SetActive(true);
    //    yield return new WaitForSeconds(0.1f);
    //    playerGetsDamaged.SetActive(false);
    //}


  
}