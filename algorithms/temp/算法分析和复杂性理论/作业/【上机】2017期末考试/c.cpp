#include <iostream>
#include <memory.h>
#include <algorithm>
#include <vector>

#define MAX 100000
using namespace std;

int buildings[MAX];
int dp1[MAX], dp2[MAX];

int solve(int n)
{
    for(int i=0; i<n; i++)
    {
        dp1[i]=1;
        dp2[i]=1;
    }
    vector<int> vec1[n], vec2[n];
    for(int i =1; i<n; i++)
    {
        for(int j =i-1; j>=0; j--)
        {
            if(buildings[j] > buildings[i])
            {
                vec1[i].push_back(j);
            }
        }
    }
    for(int i =n-2; i>=0; i--)
    {
        for(int j =i+1; j<n; j++)
        {
            if(buildings[j] > buildings[i])
            {
                vec2[i].push_back(j);
            }
        }
    }
//    for(int i = 0; i<n; i++)
//    {
//        for(int j = 0; j<vec1[i].size(); j++)
//        {
//            cout<<vec1[i][j]<<' ';
//        }
//        cout<<endl;
//    }
    int ans = 1;
    for(int i =1; i<n; i++)
    {
        int temp = 1;
        for(int j = 0; j<vec1[i].size(); j++)
        {
            if(dp1[vec1[i][j]]+1> temp)
            {
                temp = dp1[vec1[i][j]]+1;
            }
        }
        dp1[i] = temp;
        if(temp > ans)
        {
            ans = temp;
        }
    }
//    for(int i =0; i<n; i++)
//    {
//        cout<<dp1[i]<<' ';
//    }
//    cout<<endl;

    for(int i =n-2; i>=0; i--)
    {
        int temp = 1;
        for(int j = 0; j<vec2[i].size(); j++)
        {
            if(dp2[vec2[i][j]]+1> temp)
            {
                temp = dp2[vec2[i][j]]+1;
            }
        }
        dp2[i] = temp;
        if(temp > ans)
        {
            ans = temp;
        }
    }
//    for(int i =0; i<n; i++)
//    {
//        cout<<dp2[i]<<' ';
//    }
//    cout<<endl;
    return ans;
}
int main()
{
    int t, n;
    cin>>t;
    while(t--)
    {
        cin>>n;
        for(int i=0; i<n; i++)
        {
            cin>>buildings[i];
        }
//        memset(dp1, 1, sizeof(dp1));
//        memset(dp2, 1, sizeof(dp2));
        cout<< solve(n)<<endl;
    }
}
