#include <iostream>
using namespace std;
int rotationTime[5]={-1, 1, 3, 2, 0};

void ratateMap(int mp[4][4], int times)
{
    while(times--)
    {
        int mapTmp[4][4];
        for(int i=0; i<4; i++)
        {
            for(int j=0; j<4; j++)
            {
                mapTmp[j][3-i] = mp[i][j];
            }
        }
        for(int i=0; i<4; i++)
        {
            for(int j=0; j<4; j++)
            {
                mp[i][j] = mapTmp[i][j];
            }
        }
    }
}

void calculate(int mp[4][4])
{
    for(int i=0; i<4; i++)
    {
        int pos = 3;
        int j =3;
        while(j>=0)
        {
            for(int k=3; k>=0; k--)
            {
                int posTemp = k;
                while(posTemp>=0 && mp[i][posTemp]==0)
                {
                    posTemp--;
                }
                if(posTemp>=0)
                {
                    int temp2 = mp[i][posTemp];
                    mp[i][posTemp] = 0;
                    mp[i][posTemp] = temp2;
                }
            }
            if(mp[i][j] == mp[i][j-1] && mp[i][j]!= 0)
            {
                int temp = mp[i][j]*2;
                mp[i][j] = 0;
                mp[i][j-1] = 0;
                mp[i][pos] = temp;
                j=3;
                pos =3;
            }
            else
            {
                j--;
                pos--;
            }
        }
    }
}

int main()
{
    int op;
    cin>>op;
    int mp[4][4];
    for(int i=0; i<4; i++)
    {
        for(int j=0; j<4; j++)
        {
            cin>>mp[i][j];
        }
    }
    ratateMap(mp, rotationTime[op]);

    calculate(mp);

//    for(int i=0; i<4; i++)
//    {
//        for(int j=0; j<4; j++)
//        {
//            cout<<mp[i][j]<<' ';
//        }
//        cout<<endl;
//    }
    ratateMap(mp, 4-rotationTime[op]);

    for(int i=0; i<4; i++)
    {
        for(int j=0; j<4; j++)
        {
            cout<<mp[i][j]<<' ';
        }
        cout<<endl;
    }

    return 0;
}


/*
4
0 4 2 2
0 4 2 2
0 4 2 2
0 4 2 2
*/
