//OneNote Ä£Äâ->³öÕ»¼ì²é
#include <iostream>

#define MAX 1001
using namespace std;

bool judge(int *arr, int m)
{
    bool state[MAX]={false};
    int last=arr[0];
    state[last]=true;
    int lastValid;
    for(int i=1; i<m; i++)
    {
        if(arr[i]>last)
        {
            last=arr[i];
            state[last]=true;
            continue;
        }
        else
        {
            for(lastValid=last-1;lastValid>0;lastValid--)
            {
                if(state[lastValid]==false)
                {
                    break;
                }
            }
            if(arr[i]!=lastValid)
            {
                return false;
            }
            else
            {
                last=arr[i];
                state[last]=true;
            }
        }

    }
    return true;
}
int main()
{
    int n;
    int arr[MAX]={0};
    cin>>n;
    for(int i=0; i<n; i++)
    {
        int m;
        cin>>m;
        for(int j=0; j<m; j++)
        {
            cin>>arr[j];
        }
        if(judge(arr, m))
        {
            cout<<"yes"<<endl;
        }
        else
        {
            cout<<"no"<<endl;
        }
    }
    return 0;
}
