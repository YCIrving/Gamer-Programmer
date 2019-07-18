//OneNote Ä£Äâ->ºìºÚ»ıÄ¾

#include<iostream>
using namespace std;
int sumLast7(int *arr, int i)
{
    int j,sum=0;
    for (j=i;j>=i-6;j--)
    {
        sum+=arr[j];
    }
    return sum;
}
void init(int a, int b, int *arr)
{
    int i,j;
    for(i=0;i<7;i++)
    {
        arr[i]=a;
    }
    for (j=6;j>=0;j--)
    {
        if(sumLast7(arr, 6)>=0)
        {
            arr[j]=b;
        }
    }
}
int putNum(int a, int b, int* arr)
{
    int i,ans=0;
    for (i=7;i<17;i++)
    {
        arr[i]=a;
        if(sumLast7(arr, i)>=0)
            arr[i]=b;
    }
    for (i=0;i<17;i++)
    {
        ans+=arr[i];
    }
    return ans;
}
int main()
{
    int a,b;
    cin>>a>>b;
    int arr[17]={0};
    init(a,b,arr);
    int ans=putNum(a,b,arr);
    cout<<ans;
}
