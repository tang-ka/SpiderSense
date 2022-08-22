using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

//�¾ �� 
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


    public GameObject pressAnyKeyBtn; //�ƹ�Ű�� ������ ���������� �̵��ϴ� Ű��ư
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
        print("���� ��ŸƮ!");
        startButton.SetActive(false);
        optionButton.SetActive(false);
        exitButton.SetActive(false);

        loadingBar.gameObject.SetActive(true);
        isLoading = true;



        // StartCoroutine("LoadScene");
    }
    public void OnGameOption()
    {
        print("���� �ɼ�!");
        //�ɼ� â ��� MENU�� �����ְ� ������ ��ư �������
    }
    public void Exit()
    {
        print("���� ������!");
    }
    public void PressAnyKeyBtn()
    {
        if (Input.anyKey)
        {
            StartCoroutine("FadeOut");
        }
    }
    [Header("FadeOut")]
    public Image fadeOutImage; //fadeȿ���� ���Ǵ� �����̹���
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



    /*IEnumerator LoadScene() //���� �� ����ȭ
    {
        yield return null;//����ȭ�Ǹ�
        AsyncOperation op;
        op = SceneManager.LoadSceneAsync(1);
        op.allowSceneActivation = false;
        float timer = 0.0f;

        while(!op.isDone) //GameScene�� �غ� ���� ���� ����
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
