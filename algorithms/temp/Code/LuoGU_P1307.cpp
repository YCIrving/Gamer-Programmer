/*����˼����ʱ��̣ܶ����Գ��򻹿����Ż��ܶ�
������ϵ�һ��˼·������˼·�Ƕ�ȡһ���ַ�����Ȼ��ת�����
���жϵ�һ���ַ��ǲ��ǡ�-�������������������Ҳ������ת������һ���ַ�����-����
��ѭ���У�������0��ǰ������0ֱ�������´�ѭ����

������δ����������
����Ҫʹ���ַ�����ÿ��ȡ��浽��һ�����м���
int main()
{
  int n,m=0;
  scanf("%d",&n);
  while(n!=0)
  {
    m=m*10+n%10;
    n/=10;
  }
  printf("%d\n",m);
  return 0;
}
*/

#include<iostream>
#include<string>
using namespace std;
int main()
{
    string str;
    cin>>str;
    string ans=str+' ';
    int i=0;
    int flag=0,length=str.length();
    if(str[0]=='-')
        flag=1;
    for( i=0;i<length;i++)
    {
        ans[length-i]=str[i];
    }
    for(i=1;i<length;i++)
    {
        if(ans[i]!='0')
            break;
    }
    if(str[0]=='0')
        cout<<'0'<<endl;
    else
    {
        if(flag==1)
        {
            ans[i-1]='-';
            cout<<ans.substr(i-1,length-i+1);
        }
        else
        {
            cout<<ans.substr(i,length-i+1);

        }
    }
    return 0;
}

