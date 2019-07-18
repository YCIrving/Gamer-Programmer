//OneNote ƒ£ƒ‚->—≠ª∑º”√‹
#include <iostream>
#include <string>
#include <stdio.h>

#define MAX 201
using namespace std;

int getCycleList(int *arr, int n, int *cycleList)
{
    int pos=0;
    for(int i=0; i<n; i++)
    {
        pos=i;
        for(int j=1; ; j++)
        {
            pos = arr[pos];
            if(pos == i)
            {
                cycleList[i]=j;
                break;
            }
        }
    }
//    for(int i=0; i<n; i++)
//    {
//        cout<<cycleList[i]<<' ';
//    }
//    cout<<endl;
    return 0;
}
int reindex(int *arr, int *index, int *cycleList, int m, int n)
{
    int pos=0;
    for(int i=0;i<n;i++)
    {
        pos=i;
        for (int j=0; j<m%cycleList[i]; j++)
        {
            pos = arr[pos];
        }
        index[pos]=i;
        //cout<<pos<<endl;
    }
    return 0;
}
int main()
{
    int n,m;
    int arr[MAX]={0};
    int index[MAX]={0};
    int cycleList[MAX]={0};
    string s(MAX, '\0');
    while(cin>>n && n)
    {
        for(int i=0;i<n;i++)
        {
            cin>>arr[i];
            arr[i]--;
        }
        getCycleList(arr, n, cycleList);
        while(cin>>m && m)
        {
            reindex(arr, index, cycleList, m, n);
            getchar();
            getline(cin,s);
            for(int i=s.length();i<n;i++)
            {
                s[i]=' ';
            }
            for(int i=0;i<n;i++)
            {
                cout<<s[index[i]];
            }
            cout<<endl;
        }
        cout<<endl;
    }
    return 0;
}

//10
//arr:
//H e l l o _ B o b _
//4 5 3 7 2 8 1 6 10 9
//1 2 3 4 5 6 7 8 9 10
//index:
//7 5 3 1 2 8 4 6 10 9
//B o l H e o l _ _ b
//arr:
//B o l H e o l _ _ b
//4 5 3 7 2 8 1 6 10 9
//7 5 3 1 2 8 4 6 10 9
//index:
//4 2 3 7 5 6 1 8 9 10
//l e l B o _ H o b _
//1 Hello Bob
//1995 CERC
//0
//0
