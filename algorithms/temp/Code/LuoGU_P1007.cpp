/*本题有一个思考的地方值得学习，就是两个士兵在相遇时互相转身，这其实可以理解为
二人互相穿过彼此。另外，不知道为什么数组必须要开1001长度，1000长度就会出错*/
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
