using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

//태어날 때 
public class SY_UIManager : MonoBehaviour
{
    //public static int nextScene;
    public GameObject startMenu;

    [Header("Button")]
    public GameObject startButton;
    public GameObject optionButton;
    public GameObject exitButton;

    [Header("Slider")]
    public Slider loadingBar;


    public GameObject pressAnyKeyBtn; //아무키나 누르면 다음씬으로 이동하는 키버튼
    public GameObject errorBtn;

    bool isLoading;
   
    void Start()
    {
        loadingBar.gameObject.SetActive(false);
        pressAnyKeyBtn.SetActive(false);
    }
    /*public static void LoadScene(int index)
    {
        nextScene = index;
        SceneManager.LoadScene("Start Scene");
    }*/

    void Update()
    {
        StartCoroutine("ErrorBtn");
        if (isLoading == true)
        {
            loadingBar.value += Time.deltaTime;
            if (loadingBar.value == loadingBar.maxValue)
            {
                pressAnyKeyBtn.SetActive(true);
                loadingBar.gameObject.SetActive(false);
                startMenu.SetActive(false);
                PressAnyKeyBtn();
            }
        }
    }

    public void OnGameStart()
    {
        print("게임 스타트!");
        startButton.SetActive(false);
        optionButton.SetActive(false);
        exitButton.SetActive(false);

        loadingBar.gameObject.SetActive(true);
        isLoading = true;



        // StartCoroutine("LoadScene");
    }
    public void OnGameOption()
    {
        print("게임 옵션!");
        //옵션 창 기능 MENU를 보여주고 나머지 버튼 사라지게
    }
    public void Exit()
    {
        print("게임 나가기!");
    }
    public void PressAnyKeyBtn()
    {
        if (Input.anyKey)
        {
            StartCoroutine("FadeOut");
        }
    }
    [Header("FadeOut")]
    public Image fadeOutImage; //fade효과에 사용되는 검정이미지
    float time = 0f;
    float maxTime = 5f;
    IEnumerator FadeOut()
    {
        fadeOutImage.gameObject.SetActive(true);
        Color alpha = fadeOutImage.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / maxTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            fadeOutImage.color = alpha; 
            yield return null;
        }
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(1);
    }



    /*IEnumerator LoadScene() //다음 씬 동기화
    {
        yield return null;//동기화되면
        AsyncOperation op;
        op = SceneManager.LoadSceneAsync(1);
        op.allowSceneActivation = false;
        float timer = 0.0f;

        while(!op.isDone) //GameScene이 준비가 되지 않은 상태
        {
            yield return null;
            timer += Time.deltaTime;

            if(op.progress < 0.9f)
            {
                loadingBar.value = Mathf.Lerp(loadingBar.value, op.progress, timer);
                if(loadingBar.value >=op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                loadingBar.value = Mathf.Lerp(loadingBar.value, 1f, timer);
                if(loadingBar.value == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                    
                }
            }
        }

    }*/
    IEnumerator ErrorBtn()
    {
        yield return new WaitForSeconds(24f);
        Destroy(errorBtn);
    }
}
