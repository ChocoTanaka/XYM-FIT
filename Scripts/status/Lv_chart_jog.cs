using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//jog�̌C�̐ݒ�A�萔
public class Lv_chart_jog : Lv_chart
{
    //�C�̃��x�����オ��̂ɕK�v�Ȍo���l: 1.05^(LV-1) * 10�T�C�N����b�|�C���g�i�ݐρj
    int[] XFP_LV = {535,1096,1685, 2304, 2954, 3636, 4352, 5104, 5894, 6724,
        7594, 8509, 9469, 10476, 11535, 12646, 13813, 15038, 16325, 17675,
        19094, 20583, 22147, 23789, 25513, 27323, 29223, 31219, 33315};

    //�C�̏C���ɕK�v�ȃ|�C���g (0.67 -0.014*MEN)*5�T�C�N����b�|�C���g
    int[] MEN = {187,183, 179, 175, 171, 167, 163, 159, 155, 151, 147, 143, 139, 135, 131, 127, 123, 119, 115, 111};

    //���[�^�[�̍ő�l
    int matorMAX = 14;

    //�|�C���g�v�Z��
    int average = 7;
    double stdv_in = 3;
    double stdv_out = 2;
    public int GetXFP_LF(int Lv)
    {

        return XFP_LV[Lv - 1];
    }

    public int GetMEN(int MENLv)
    {
        return MEN[MENLv - 1];
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
        if (speed < average - stdv_in || speed > average + stdv_in)
        {
            return stdv_out;
        }
        else
        {
            return stdv_in;
        }
    }
}
