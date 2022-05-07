using UnityEngine;
using System.Collections;
using System;

public class LocationUpdater : MonoBehaviour
{
    public float IntervalSeconds = 1.0f;
    public LocationServiceStatus Status;
    public LocationInfo Location;

    public Gamemanager GM;
    //�o�x
    double lat1, lat2;
    //�ܓx
    double lon1, lon2;

    double distance, distance_All, Distance60,Distance60minus;
    double speed, speed60;

    int timelog;

    bool Switch;

    private void Start()
    {
        Switch = false;
        timelog = 0;
        Distance60minus = 0;
        StartCoroutine("CheckLocation");
        StartCoroutine("CheckSpeed60");
    }


    private void Update()
    {
        Switch = GM.GetSwitch();

        timelog = GM.GetTime();

    }

    //GPS�@�\�A���i�g���Ƃ��̓A�v���̈ʒu�@�\��A�����邱�Ɓj
    IEnumerator CheckLocation()
    {
        while (true)
        {
            if(Switch == true)
            {
                this.Status = Input.location.status;
                if (Input.location.isEnabledByUser)
                {
                    switch (this.Status)
                    {
                        case LocationServiceStatus.Stopped:
                            Input.location.Start();
                            break;
                        case LocationServiceStatus.Running:
                            this.Location = Input.location.lastData;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    // FIXME �ʒu����L���ɂ���!! �I�ȃ_�C�A���O�̕\������������Ɨǂ�����
                    Debug.Log("location is disabled by user");
                }

                if(lat1 == 0 && lon1 == 0)
                {
                    lat1 = Location.latitude;
                    lon1 = Location.longitude;
                }
                else
                {
                    lat1 = lat2;
                    lon1 = lon2;
                }

                lat2 = Location.latitude;
                lon2 = Location.longitude;

                distance_All += calcdistance(lat1, lon1, lat2, lon2);
                distance = calcdistance(lat1, lon1, lat2, lon2);
            }

            // �w�肵���b����ɍēx����𑖂点��
            yield return new WaitForSeconds(IntervalSeconds);
        }
    }

    IEnumerator CheckSpeed60()
    {
        while (true)
        {
            if(GM.GetSwitch() == true)
            {
                yield return new WaitForSeconds(60);
                //60�b���Ƃ�60�b�Ői�񂾋������v�Z����
                //�i60�b�Ői�񂾋����j=�i�S�����j-�i���߂�60�b�O�܂Ői�񂾋����j
                Distance60 = distance_All - Distance60minus;
                Distance60minus = distance_All;

                speed60 = Distance60 * 60;
                
            }
            else
            {
                yield return null;
            }
        }

    }

    //renderer�A���p
    public double GetdistanceAll()
    {
        return distance_All;
    }

    //renderer�A���p


    public double GetSpeed60()
    {
        return speed60;
    }

    //�����ɏ]���������o���Bdouble�Ȃ̂͌����̖��
    double calcdistance(double lat1, double lon1, double lat2, double lon2)
    {
        double rad = Math.PI/180;

        double th = Math.Cos(lat1 * rad) * Math.Cos(lat2 * rad) * Math.Cos((lon2 - lon1) * rad) + Math.Sin(lat1 * rad) * Math.Sin(lat2 * rad);

        //NaN�΍��Clamp
        if(th > 1)
        {
            th = 1;
        }else if (th < -1)
        {
            th = -1;
        }

        double dist = 6371 * Math.Acos(th);
        return dist;
    }
}