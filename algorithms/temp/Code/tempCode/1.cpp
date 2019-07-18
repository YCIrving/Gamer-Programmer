// C++ code for calculating number of ways
// to distribute m mangoes amongst n people
// where all mangoes and people are identical
#include <bits/stdc++.h>
#include <vector>
using namespace std;

vector<int> stk;

// function used to generate binomial coefficient
// time complexity O(m)
int binomial_coefficient(int n, int m)
{
    int res = 1;

    if (m > n - m)
        m = n - m;

    for (int i = 0; i < m; ++i) {
        res *= (n - i);
        res /= (i + 1);
    }

    return res;
}

// helper function for generating no of ways
// to distribute m mangoes amongst n people
int calculate_ways(int m, int n)
{
    // not enough mangoes to be distributed
//    if (m < n)
//        return 0;

    // ways  -> (n+m-1)C(n-1)
    int ways = binomial_coefficient(n + m - 1, n - 1);
    return ways;
}
void DFS(int m, int n) //还有m个礼物，n个人
{
    if(n != 1) {
        for(int i=m; i>=0; i--) {
            stk.push_back(i);
            DFS(m-i, n-1);
            stk.pop_back();
        }
    }
    else {
        stk.push_back(m);
        for(int i=0; i<stk.size()-1; i++)
        {
            for(int j=0; j<stk[i]; j++) {
                cout<<"* ";
            }
            cout<<"| ";
        }
        for(int j=0; j<stk[stk.size()-1]; j++) {
            cout<<"* ";
        }
        cout<<endl;
        stk.pop_back();
        return;
    }
}
// Driver function
int main()
{
    // m represents number of mangoes
    // n represents number of people
    int m = 7, n = 5;
    cin>>m>>n;
    int result = calculate_ways(m, n);
    printf("%d\n", result);
    DFS(m, n);
    return 0;
}
