/*������һ��˼���ĵط�ֵ��ѧϰ����������ʿ��������ʱ����ת������ʵ�������Ϊ
���˻��ഩ���˴ˡ����⣬��֪��Ϊʲô�������Ҫ��1001���ȣ�1000���Ⱦͻ����*/
#include<iostream>
using namespace std;
int main()
{
    int l,n,s[10000]={0};
    cin>>l>>n;
    for(int i=0;i<n;i++)
        cin>>s[i];
    int mid=l/2,max=0,min=0;
    for(int i=0;i<n;i++)
    {
        if(s[i]>mid)
        {
            if(min<l+1-s[i])
                min=l+1-s[i];
            if(max<s[i])
                max=s[i];
        }
        else
        {
            if(min<s[i])
                min=s[i];
            if(max<l+1-s[i])
                max=l+1-s[i];
        }
    }
    cout<<min<<' '<<max;
    return 0;
}
