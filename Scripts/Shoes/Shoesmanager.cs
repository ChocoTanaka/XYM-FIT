using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoesmanager : MonoBehaviour
{

    int num;


    public GameObject Content;
    public GameObject ButtonPrefab;

    ShoesButton ShoesButton;

    public Text Shoesnowtext;

    private void Start()
    {
        Content = GameObject.Find("Content");

        for (int i = 0; i < Shoesbox.GetShoescase().Count; i++)
        {
            ButtonPrefab = (GameObject)Instantiate(ButtonPrefab);
            ButtonPrefab.transform.SetParent(Content.transform, false);
            ShoesButton = ButtonPrefab.GetComponent<ShoesButton>();
            ShoesButton.GSnum = i + 1;
        }
    }

    private void Update()
    {
        num = Shoesbox.GetShoescase().IndexOf(Shoesbox.GetShoesnow());

        if(num == -1)
        {
            Shoesnowtext.text = "No Shoes";
        }
        else
        {
            num = num + 1;
            Shoesnowtext.text = "Shoes(Now:Shoes" + num.ToString("0") + ")";
        }
        

    }

    //Make Shoes�{�^���������ƌC�������
    public void MakeShoes()
    {
        Shoes Shoes = new Shoes();
        Setstatus(Shoes);
        Shoesbox.AddShoescase(Shoes);

        ButtonPrefab = (GameObject)Instantiate(ButtonPrefab);
        ButtonPrefab.transform.SetParent(Content.transform, false);
        ShoesButton = ButtonPrefab.GetComponent<ShoesButton>();
        ShoesButton.GSnum = Shoesbox.GetShoescase().Count;
    }

    //�����X�e�[�^�X�ݒ�
    void Setstatus(Shoes Shoes)
    {
        //walk��jog��run��(�I�𐧂ɂ��邩�����_���ɂ��邩�͗v����)
        int randel = Random.Range(0, 100);
        int randrare = Random.Range(0, 100);
        //60%��walk�A40%��jog
        if (randel < 60)
        {
            Shoes.GSElement = Shoes.Elementenum.walk;
        }
        else
        {
            Shoes.GSElement = Shoes.Elementenum.jog;
        }

        //60%�ŃR�����A35%�ŃA���R�����A5%�Ń��A

        if(randrare < 60)
        {
            Shoes.GSRareity = Shoes.Rareityenum.common;
        }
        else if(randrare > 60 && randrare < 94)
        {
            Shoes.GSRareity = Shoes.Rareityenum.uncommon;
            Shoes.GSpoint += 5;
        }
        else
        {
            Shoes.GSRareity = Shoes.Rareityenum.rare;
            Shoes.GSpoint += 10;
        }

        //10�|�C���g��
        for (int i =0; i<10; i++)
        {
            //0~99�܂Œ��I�ŁA����U��
            int rand = Random.Range(0, 100);

            if(rand %4 == 0)
            {
                Shoes.GSRESLV = Shoes.GSRESLV + 1;
            }
            if (rand % 4 == 1)
            {
                Shoes.GSMENLV = Shoes.GSMENLV + 1;
            }
            if (rand % 4 == 2)
            {
                Shoes.GSLUCLV = Shoes.GSLUCLV + 1;
            }
            if (rand % 4 == 3)
            {
                Shoes.GSEFFLV = Shoes.GSEFFLV + 1;
            }
        }
    }
}
