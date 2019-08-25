#include <iostream>
#include <string>
#define MAXN 65540

using namespace std;

int mid[MAXN];
int post[MAXN];

void solve(int s_mid, int e_mid, int s_post, int e_post)
{
    cout<<post[e_post]<<' ';
//    cout<<s_mid<< e_mid<<s_post<<e_post<<endl;
    if(s_post == e_post) return;
    int pos =-1;
    for(int i=s_mid; i<=e_mid; i++)
    {
        if(mid[i] == post[e_post])
        {
            pos = i;
            break;
        }
    }
//    cout<<s_mid<<pos-1<<s_post<<s_post + (pos-s_mid)-1<<endl;
    if(pos != s_mid) //×ó×ÓÊ÷
    {
        solve(s_mid, pos-1, s_post, s_post + (pos-s_mid)-1);
    }
//    cout<<pos+1<<e_mid<<s_post+(e_mid - pos)<<e_post-1<<endl;
    if(pos != e_mid) //ÓÒ×ÓÊ÷
    {
        solve(pos+1, e_mid, s_post+(pos - s_mid), e_post-1);
    }
}

int main()
{
    string str1, str2;
    getline(cin, str1);
    getline(cin, str2);
    int n=0;
    int cnt =0;
    for(int i=0; i<str1.length(); i++)
    {
        if(str1[i] == ' ')
        {
            mid[cnt++] = n;
            n=0;
        }
        else
        {
            n= n*10 + str1[i] - '0';
        }
    }
    mid[cnt++] = n;
    n=0;
    cnt = 0;
    for(int i=0; i<str2.length(); i++)
    {
        if(str2[i] == ' ')
        {
            post[cnt++] = n;
            n=0;
        }
        else
        {
            n= n*10 + str2[i] - '0';
        }
    }
    post[cnt++] = n;
    solve(0, cnt-1, 0, cnt-1);
//    for(int i=0; i<cnt; i++)
//    {
//        cout<<mid[i]<< ' ';
//    }
//    cout<<endl;
//    for(int i=0; i<cnt; i++)
//    {
//        cout<<post[i]<< ' ';
//    }
//    cout<<cnt<<endl;

}
