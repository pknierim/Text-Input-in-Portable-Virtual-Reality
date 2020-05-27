using UnityEngine;
using System.Collections;

public class KeyMover : MonoBehaviour
{
    Vector3 moveDown = new Vector3(0, -0.03604493f, 0);
    Vector3 moveUp = new Vector3(0, 0.03604493f, 0);

    GameObject kb0;
    GameObject kb1;
    GameObject kb2;
    GameObject kb3;
    GameObject kb4;
    GameObject kb5;
    GameObject kb6;
    GameObject kb7;
    GameObject kb8;
    GameObject kb9;
    GameObject kbQ;
    GameObject kbW;
    GameObject kbE;
    GameObject kbR;
    GameObject kbT;
    GameObject kbZ;
    GameObject kbU;
    GameObject kbI;
    GameObject kbO;
    GameObject kbP;
    GameObject kbA;
    GameObject kbS;
    GameObject kbD;
    GameObject kbF;
    GameObject kbG;
    GameObject kbH;
    GameObject kbJ;
    GameObject kbK;
    GameObject kbL;
    GameObject kbY;
    GameObject kbX;
    GameObject kbC;
    GameObject kbV;
    GameObject kbB;
    GameObject kbN;
    GameObject kbM;
    GameObject kbLShift;
    GameObject kbRShift;
    GameObject kbSpace;
    GameObject kbEnter;
    GameObject kbEsc;
    GameObject kbBackspace;
    GameObject kbAltLeft;
    GameObject kbAltRight;
    GameObject kbCapsLock;
    GameObject kbLeft;
    GameObject kbRight;
    GameObject kbUp;
    GameObject kbDown;
    GameObject kbApostrophe;
  
    // Use this for initialization
    void Start()
    {
        kb0 = GameObject.Find("AppleKB_0");
        kb1 = GameObject.Find("AppleKB_1");
        kb2 = GameObject.Find("AppleKB_2");
        kb3 = GameObject.Find("AppleKB_3");
        kb4 = GameObject.Find("AppleKB_4");
        kb5 = GameObject.Find("AppleKB_5");
        kb6 = GameObject.Find("AppleKB_6");
        kb7 = GameObject.Find("AppleKB_7");
        kb8 = GameObject.Find("AppleKB_8");
        kb9 = GameObject.Find("AppleKB_9");

        kbA = GameObject.Find("AppleKB_A");
        kbB = GameObject.Find("AppleKB_B");
        kbC = GameObject.Find("AppleKB_C");
        kbD = GameObject.Find("AppleKB_D 1");
        kbE = GameObject.Find("AppleKB_E");
        kbF = GameObject.Find("AppleKB_F 1");
        kbG = GameObject.Find("AppleKB_G");
        kbH = GameObject.Find("AppleKB_H");
        kbI = GameObject.Find("AppleKB_I");
        kbJ = GameObject.Find("AppleKB_J");
        kbK = GameObject.Find("AppleKB_K");
        kbL = GameObject.Find("AppleKB_L");
        kbM = GameObject.Find("AppleKB_M");
        kbN = GameObject.Find("AppleKB_N");
        kbO = GameObject.Find("AppleKB_O");
        kbP = GameObject.Find("AppleKB_P");
        kbQ = GameObject.Find("AppleKB_Q");
        kbR = GameObject.Find("AppleKB_R");
        kbS = GameObject.Find("AppleKB_S");
        kbT = GameObject.Find("AppleKB_T");
        kbU = GameObject.Find("AppleKB_U");
        kbV = GameObject.Find("AppleKB_V");
        kbW = GameObject.Find("AppleKB_W");
        kbX = GameObject.Find("AppleKB_X");
        kbY = GameObject.Find("AppleKB_Y");
        kbZ = GameObject.Find("AppleKB_Z");

        kbLShift = GameObject.Find("AppleKB_Shift_Left");
        kbRShift = GameObject.Find("AppleKB_Shift_Right");
        kbSpace = GameObject.Find("AppleKB_Space");
        kbEnter = GameObject.Find("AppleKB_Enter");
        kbEsc = GameObject.Find("AppleKB_ESCt");
        kbBackspace = GameObject.Find("AppleKB_Backspace");
        kbAltLeft = GameObject.Find("AppleKB_Alt_Left");
        kbAltRight = GameObject.Find("AppleKB_Alt_Right");
        kbCapsLock = GameObject.Find("AppleKB_CapsLock");
        kbLeft = GameObject.Find("AppleKB_Left");
        kbRight = GameObject.Find("AppleKB_Right");
        kbUp = GameObject.Find("AppleKB_Top");
        kbDown = GameObject.Find("AppleKB_Down");
        kbApostrophe = GameObject.Find("AppleKB_Sharp");

    }

    //0.06390014
    //0.09994507

    // Update is called once per frame
    void Update()
    {
        //var input = Input.inputString;
        //switch (input)
        //{
        //    case "0":
        //        Debug.Log("0 was pressed");
        //        break;
        //}
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            kb0.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            kb0.transform.localPosition += moveUp;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            kb1.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            kb1.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            kb2.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            kb2.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
           kb3.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            kb3.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            kb4.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            kb4.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            kb5.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            kb5.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            kb6.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            kb6.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            kb7.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            kb7.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            kb8.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            kb8.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            kb9.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            kb9.transform.localPosition += moveUp;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            kbA.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            kbA.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            kbAltLeft.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            kbAltLeft.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            kbAltRight.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.RightAlt))
        {
            kbAltRight.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            kbBackspace.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
           kbBackspace.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            kbB.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            kbB.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            kbC.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            kbC.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            kbD.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            kbD.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            kbE.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            kbE.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            kbF.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            kbF.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            kbG.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            kbG.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            kbH.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            kbH.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            kbI.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            kbI.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            kbJ.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            kbJ.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            kbK.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            kbK.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            kbL.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            kbL.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            kbM.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
           kbM.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            kbN.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.N))
        {
            kbN.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            kbO.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            kbO.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            kbP.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            kbP.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            kbQ.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            kbQ.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            kbR.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            kbR.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            kbS.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            kbS.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            kbT.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            kbT.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            kbU.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.U))
        {
            kbU.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            kbV.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            kbV.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            kbW.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            kbW.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            kbX.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            kbX.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
           kbY.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Y))
        {
            kbY.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            kbZ.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            kbZ.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            kbSpace.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            kbSpace.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            kbLShift.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            kbLShift.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            kbRShift.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.RightShift))
        {
            kbRShift.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            kbEsc.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            kbEsc.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            kbEnter.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            kbEnter.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            kbCapsLock.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.CapsLock))
        {
            kbCapsLock.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            kbLeft.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            kbLeft.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            kbRight.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            kbRight.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            kbUp.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            kbUp.transform.localPosition += moveUp;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            kbDown.transform.localPosition += moveDown;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            kbDown.transform.localPosition += moveUp;
        }
        //if (Logging.inputKey == '#' || Logging.inputKey == 39)
        //{
        //    kbApostrophe.transform.localPosition += moveDown;
        //}
        //if (Logging.inputKey == '#' || Logging.inputKey == 39)
        //{
        //    kbApostrophe.transform.localPosition += moveUp;
        //}


    }

}
