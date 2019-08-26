/*

2
19
7

true
true

1
5995895895859
false

*/


#include <iostream>
#include <string.h>

using namespace std;

bool flag[1010];

bool helper (int n)
{
    //cout<<n<<endl;
    int sum =0;
    if(n == 1)
        return true;
    flag[n] = true;

    while(n!=0)
    {
        sum += (n%10) * (n%10);
        n/=10;
    }

    if(flag[sum]) return false;
    return helper(sum);

}
bool judge(int n)
{
    int sum = 0;
    memset(flag, 0, sizeof(flag));
    while(n!=0)
    {
        sum += (n%10) * (n%10);
        n/=10;
    }
    return helper(sum);
}

int main()
{
    int t;
    cin>>t;
    while(t--)
    {
        int n;
        cin>>n;
        if(judge(n)) cout<<"true"<<endl;
        else cout<<"false"<<endl;
    }
    return 0;
}
