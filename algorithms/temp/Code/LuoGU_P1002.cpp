/*
������Ҫע�������㣺
1.���ݽϴ�ʱ������long long int ����ý����Ҳ����double������Ҫע�ⲻ�ÿ�ѧ�������������С��������λ��
2.���������������±��ת���������׳���һ����Ҫ���⣬Ŀǰ��Ϊ��õķ���������д����ʱֱ�ӽ���λ������������
3.�������Լ���һ���ö�̬�滮���㷨��ȫ���Լ�������ģ����гɾ͸У���Ȼ����˵�ⲻ���Ƕ��棬���ǻ�����һ����˼���������^_^��
*/

#include<iostream>
//#include <iomanip>
using namespace std;
int main()
{
    int x_b,y_b,x_h,y_h;
    long long map[21][21]={0};
    map[0][0]=1;//��ͼ��ʼ��
    cin>>x_b>>y_b>>x_h>>y_h;
    //����������߷�Χ
    map[y_h][x_h]=-1;//x��y�����������ת��ʮ����Ҫ������
    if(x_h-2<=x_b&&y_h+1<=y_b)
        map[y_h+1][x_h-2]=-1;
    if(x_h-2<=x_b&&y_h-1<=y_b)
        map[y_h-1][x_h-2]=-1;
    if(x_h+2<=x_b&&y_h+1<=y_b)
        map[y_h+1][x_h+2]=-1;
    if(x_h+2<=x_b&&y_h-1<=y_b)
        map[y_h-1][x_h+2]=-1;
    if(x_h-1<=x_b&&y_h+2<=y_b)
        map[y_h+2][x_h-1]=-1;
    if(x_h-1<=x_b&&y_h-2<=y_b)
        map[y_h-2][x_h-1]=-1;
    if(x_h+1<=x_b&&y_h+2<=y_b)
        map[y_h+2][x_h+1]=-1;
    if(x_h+1<=x_b&&y_h-2<=y_b)
        map[y_h-2][x_h+1]=-1;
    for(int i=0;i<=y_b;i++)//iΪ������������,jΪ������������
        for(int j=0;j<=x_b;j++)
    {
        if(map[i][j]!=-1)
        {
            if(i-1>=0&&map[i-1][j]!=-1)
                map[i][j]+=map[i-1][j];
            if(j-1>=0&&map[i][j-1]!=-1)
                map[i][j]+=map[i][j-1];
        }
        //cout<<'('<<j<<','<<i<<')'<<':'<<map[i][j]<<endl;
    }
    cout<<map[y_b][x_b];//std::fixed<<setprecision(0)<<map[x_b][y_b];//<<;
    return 0;
}

