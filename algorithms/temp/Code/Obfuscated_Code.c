#include <windows.h>
#include <stdio.h>
char* title ="I Hate Obfuscated Code!";
char* s1="\u7b2c\u5341\u5c4a\u79d1\u6280\u6587\u5316\u8282";
char* s2="\u6df7\u4e71\u7f16\u7801\u5927\u8d5b";
char* s3="\u8f6f";
char* s4="\u73ed";
char* s5="\u9ad8\u5b9c\u741b";
COORD c1={23,1},c2={26,17},c3={26,19},c4={33,20};

void position(COORD _){SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE),_);}

void color(const unsigned short _){SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), _);}

int o_o(int _){SetConsoleTitle(title);while(_<=14){color(_);printf("       %s>_<     ",title);
_%(*(">_<")-'<')?*(">_<"):putchar(10);_=_+*(">_<"+2)-';';sleep(1);}}

int o__o(int _){system("cls");int OoO,o0o;for(OoO=0;OoO<18;OoO++){o0o=0;
    if(OoO<9){
        _=20+OoO;while(_--)putchar(32);putchar(92);
        _=38-2*OoO;while(_--)putchar(32);putchar(47);}
    else{
        _=37-OoO;while(_--)putchar(32);putchar(47);
        _=2*OoO+4;while(_--)putchar(32);putchar(92);}putchar(10);}
    _=30;while(_--)putchar(32);_=20;while(_--)putchar('-');sleep(2);
    position(c1);putchar('A');_=10;while(_--)putchar('h');putchar('!');printf(title);sleep(2);position(c2);
}
int o_0(int _)
{_^*("o_0"+(*"o_o">>6))-'!'-'!'?putchar(_^*&("^_^"[2])-' '-'!'?
*(&"y^0^yQplcwgjel*\\~dzj~1Pj.LU^josu{y^_^y"[_]+4)^_-0:10),o_0(_+*("o.o!"+3)-' '):*"0.0";}

int o__0(int _)
{sleep(1);color(10);position(c3);printf("%s %s",s1,s2);
position(c4);printf("%s1309%s  %s",s3,s4,s5);}

int main()
{
    o_o(1);
    o__o(1);
    o_0(1);
    o__0(1);
    return 0;
}

