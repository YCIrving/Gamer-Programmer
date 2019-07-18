#include <iostream>
#include <vector>

using namespace std;

int main()
{
    int n, num, k;
    vector<int> linkedList;

    cin>>n;
    for(int i=0; i<n; i++)
    {
        cin>>num;
        linkedList.push_back(num);
    }
    cin>>k;
    if(k>=num)
    {
        cout<<"null";
    }
    else
    {
        cout<<linkedList[num - k - 1];
    }
    return 0;
}
