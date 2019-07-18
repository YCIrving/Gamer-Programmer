// Onenote 动态规划: 合唱队形
#include <iostream>
#include <math.h>
#include <vector>
using namespace std;

//只记录第一个比该元素小的不行
//两遍可以用一组代码

int arr[102]={0}, f_pre[102]={0}, f_post[102]={0};
struct pre
{
    vector <int> v;
}pres[102];
struct post
{
    vector <int> v;
}posts[102];
int main()
{
    int n;
    cin>>n;
//    pres[1].v.push_back(0);
    f_pre[0]=0;
//    posts[n].v.push_back(n+1);
    f_post[n+1]=0;
//    pres[1].v.push_back(0);
//    posts[n].v.push_back(n+1);
    for(int i=1;i<=n;i++)
    {
        cin>>arr[i];
    }
//预计算每个元素之前比它小的元素有哪些
    for(int i=2;i<=n;i++)
    {
        for(int j=i-1;j>=1;j--)
        {
            if(arr[j]<arr[i])
            {
                pres[i].v.push_back(j);
            }
            else
            {
                for(int k=0;k<pres[j].v.size();k++)
                {
                    if(arr[pres[j].v[k]]<arr[i])
                    {
                        pres[i].v.push_back(pres[j].v[k]);
                    }
                }
                break;
            }
        }
    }
//预计算每个元素之后比它小的元素有哪些
    for(int i=n-1;i>=1;i--)
    {
        for(int j=i+1;j<=n;j++)
        {
            if(arr[j]<arr[i])
            {
                posts[i].v.push_back(j);
            }
            else
            {

                for(int k=0;k<posts[j].v.size();k++)
                {
                    if(arr[posts[j].v[k]]<arr[i])
                    {
                        posts[i].v.push_back(posts[j].v[k]);
                    }
                }
                break;
            }
        }
    }

//    for(int i=1;i<=n;i++)
//    {
//        cout<<i<<":"<<endl;
//        for(int j=0;j<pres[i].v.size();j++)
//        {
//            cout<<pres[i].v[j]<<' ';
//        }
//        cout<<endl;
//        for(int j=0;j<posts[i].v.size();j++)
//        {
//            cout<<posts[i].v[j]<<' ';
//        }
//        cout<<endl<<endl;
//    }

//计算两个f值
    for(int i=1;i<=n;i++)
    {
        f_pre[i]=1;
        for(int j=0;j<pres[i].v.size();j++)
        {
            if(f_pre[pres[i].v[j]]+1>f_pre[i])
            {
                f_pre[i]=f_pre[pres[i].v[j]]+1;
            }
        }
    }
    for(int i=n;i>=1;i--)
    {
        f_post[i]=1;
        for(int j=0;j<posts[i].v.size();j++)
        {
            if(f_post[posts[i].v[j]]+1>f_post[i])
            {
                f_post[i]=f_post[posts[i].v[j]]+1;
            }
        }
    }

    int ans=n,temp;
    for(int i=1;i<=n;i++)
    {
        temp=f_pre[i]+f_post[i]-1;
        temp=n-temp;
        if(temp<ans)
        {
            ans=temp;
        }
    }
//    for(int i=1;i<=n;i++)
//    {
//        cout<<f_pre[i]<<" "<<f_post[i]<<endl;
//    }
    cout<<ans<<endl;
    return 0;


}
