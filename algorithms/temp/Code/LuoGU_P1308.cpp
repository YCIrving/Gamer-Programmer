/*
�����ص����ڴ����������һ������
*/

#include<iostream>
#include<string>
#include <algorithm>//���ڴ�Сдת��
using namespace std;
int main()
{
    int ans=0,pos=0,length=0;
    int flag,k;
    string word,temp;
    cin>>word;
    transform(word.begin(), word.end(), word.begin(), ::tolower);//��Сдת������,��дΪupper
    getline(cin,temp);
    getline(cin,temp);
    transform(temp.begin(), temp.end(), temp.begin(), ::tolower);//��Сдת������
    for(int i=0;i<temp.length();i++)
    {
        if(temp[i]==word[0]&&(i-1<0||temp[i-1]==' '))
        {
            flag=1;
            k=i+1;
            for(int j=1;j<word.length()&&k<temp.length();j++,k++)
            {
                if(word[j]!=temp[k])
                {
                    flag=0;
                    break;
                }
            }
        }
        if(flag==1&&temp[k]==' ')
        {
            ans++;
            flag=0;
            if(ans==1)
                pos=i;
        }
    }
    if(ans==0)
        cout<<"-1";
    else
        cout<<ans<<' '<<pos;
    return 0;
}
