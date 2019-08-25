#include <queue>
#include <iostream>
#include <vector>
#include <math.h>
#include <stdio.h>

using namespace std;
int main()
{
    int n;
    double temp1, temp2;
    priority_queue<double, vector<double>, less<double> > q;
    cin>>n;
    for(int i=0; i<n; i++)
    {
        cin>>temp1;
        q.push(temp1);
    }
    while(q.size()>=2)
    {
        temp1 = q.top();
        q.pop();
        temp2 = q.top();
        q.pop();
        q.push(2*sqrt(temp1*temp2));
    }
    printf("%.3f\n", q.top());
}
