using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//walk�̌C�̐ݒ�A�萔
public class Lv_chart_walk : Lv_chart
{
    //�C�̃��x�����オ��̂ɕK�v�Ȍo���l: 1.05^(LV-1) * 10�T�C�N����b�|�C���g�i�ݐρj
    int[] XFP_LV = { 291,597, 918, 1256, 1610, 1982, 2372, 2782, 3212, 3664,
        4139, 4637, 5160, 5710, 6287, 6892, 7528, 8196, 8897, 9633,
        10406, 11218, 12070, 12965, 13905, 14891, 15927, 17015, 18157};

    //�C�̏C���ɕK�v�ȃ|�C���g (0.67 -0.014*MEN)*5�T�C�N����b�|�C���g
    int[] MEN = {109, 107, 104, 102, 100, 97,  95,  93,  90,  88,  86,  83,  81,  79,  76,  74,  72,  69,  67,  65};

    //���[�^�[�̍ő�l
    int matorMAX = 8;

    //�|�C���g�v�Z��
    int average = 4;
    double stdv_in = 2;
    double stdv_out = 1.33;


    public int GetXFP_LF(int Lv)
    {
        
        return XFP_LV[Lv-1];
    }

    public int GetMEN(int MENLv)
    {
        return MEN[MENLv-1];
    }

    public int GetMatorMax()
    {
        return matorMAX;
    }

    public int Getave()
    {
        return average;
    }

    //stdv�͈͓̔��Ȃ�stdv_in�A�͈͊O�Ȃ�stdv_out
    public double Getstdv(double speed)
    {
        if (speed < average-stdv_in || speed > average + stdv_in)
        {
            return stdv_out;
        }
        else
        {
            return stdv_in;
        }
    }
}
