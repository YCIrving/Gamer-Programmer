/*
������ص��������Ŀ�ϣ�
1.n����Ϊż���������޷��������
2.n��Ӳ����Ҫ��תn�Σ�
3.��i�η�תʱ�����˵�i��Ӳ���⣬����Ӳ�Ҷ���Ҫ��ת��
4.��ת������a[j]=1-a[j]%2;���ʽ������ع��졣
*/

#include<iostream>
#include<string>
using namespace std;
int main()
{
    int n=0;
    int a[101]={0};
    cin>>n;
    cout<<n<<endl;
    for(int i=0;i<n;i++)
    {
        for(int j=0;j<n;j++)
        {
            if(j!=i)
                a[j]=1-a[j]%2;
            cout<<a[j];
        }
        cout<<endl;
    }
    return 0;
}
