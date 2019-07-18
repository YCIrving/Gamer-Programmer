/*
很简单的模拟，利用cin读取数据真的十分方便
*/

#include<iostream>
#include<string>
using namespace std;
class student
{
public:
    string name;
    int grade;
    int score;
    char job;
    char west;
    int paper;
    int scholarship;
    void setInfo(string name,int grade,int score,char job,char west,int paper)
    {
        this->name=name;
        this->grade=grade;
        this->score=score;
        this->job=job;
        this->west=west;
        this->paper=paper;
        scholarship=0;
        if(grade>80&&paper>=1)
            scholarship+=8000;
        if(grade>85&&score>80)
            scholarship+=4000;
        if(grade>90)
            scholarship+=2000;
        if(west=='Y'&&grade>85)
            scholarship+=1000;
        if(job=='Y'&&score>80)
            scholarship+=850;
    }
};
int main()
{
    student s[101];
    int n=0;
    string name;
    char job,west;
    int grade,score,paper;
    int max=0,j=0,sum=0;
    cin>>n;
    for(int i=0;i<n;i++)
    {
        cin>>name>>grade>>score>>job>>west>>paper;
        s[i].setInfo(name,grade,score,job,west,paper);
    }
    for(int i=0;i<n;i++)
    {
        sum+=s[i].scholarship;
        if(max<s[i].scholarship)
        {
            max=s[i].scholarship;
            j=i;
        }
    }
    cout<<s[j].name<<endl<<s[j].scholarship<<endl<<sum;
    return 0;
}
