/*
�����ֵ��ע����ǣ������int����������������ᳬʱ��
�������unsigned int ������������򲻻ᳬʱ

*/
#include<iostream>
using namespace std;
int main()
{
    std::ios::sync_with_stdio(false);//�ӿ�cin��cout
    unsigned int n=0,page;//�ؼ��������壬������ȡ��ͳ���ʱ��unsigned intҪ��int��һЩ
    unsigned int ans[10]={0};
    cin>>n;
    for(unsigned int i=1;i<=n;i++)
    {
        page=i;
        while(page)
        {
            ans[page%10]++;
            page/=10;
        }
    }
    for(unsigned int i=0;i<10;i++)
        cout<<ans[i]<<endl;
    return 0;
}
