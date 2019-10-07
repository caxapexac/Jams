#define TRUE 1
#define FALSE 0
#define STRING char*
#define WSTRING wchar_t*


/* Maths */

#define PI                  3.14159265
#define RAD2DEG(x)          ((x)/PI*180)
#define DEG2RAD(x)          ((x)*PI/180)
#define ALIGNB(x, align)    (((x) + ((align) - 1)) & ~((align) - 1))
#define ALIGN(x, align)     ((((x) + ((align) - 1)) / (align)) * (align))
#define FLOORB(x, align)    ((x) & ~((align) - 1))
#define FLOOR(x, align)     (((x) / (align)) * (align))
#define CEILB(x, align)     ALIGNB(x, align)
#define CEIL(x, align)      ALIGN(x, align)
#define CLIP(x, min, max)   (((x) < (min)) ? (min) : \
                            (((x) > (max)) ? (max) : (x)))
#define UCLIP(x, max)       (((x) > (max)) ? (max) : (x))
#define LCLIP(x, min)       (((x) < (min)) ? (min) : (x))
#define MIN(x, y)           (((x) < (y)) ?  (x) : (y))
#define MAX(x, y)           (((x) > (y)) ?  (x) : (y))
#define ABS(x)              (((x) <  0) ? -(x) : (x))
#define DIFF(a,b)           ABS((a)-(b))
#define IS_NAN(x)            ((x) != (x)) //является ли это число NaN
#define SIGN(x)             COMPARE(x, 0) //возвращает знак числа
#define IS_ODD( num )       ((num) & 1) //четное ли число
#define IS_EVEN( num )      (!IS_ODD( (num) )) //нечетное ли число
#define IS_BETWEEN(n,L,H)   ((unsigned char)((n) >= (L) && (n) <= (H)))

#define LSB(x) ((x) ^ ((x) - 1) & (x)) //least significant bit
#define NP2(x) (--(x), (x)|=(x)>>1, (x)|=(x)>>2, (x)|=(x)>>4, (x)|=(x)>>8, (x)|=(x)>>16, ++(x)) //nearest upper power of 2

#define LERP_INT (a, b, f) (a + (int)(f * (float)(b-a))) //a < b - integers, f - float between 0..1
#define LEPR_FLOAT(a, b, f) (a + f * (b - a)) //a < b - floats, f - float between 0..1


/* Algorithms */

#define FOREVER for(;;)
#define FOREACH(item, array) \
    for(int keep=1, \
            count=0,\
            size=sizeof (array)/sizeof *(array); \
        keep && count != size; \
        keep = !keep, count++) \
      for(item = (array)+count; keep; keep = !keep)

#define NEW(type, n) ( (type *) malloc(1 + (n) * sizeof(type)) ) //привет ООП ;)

#define IMPLIES(x, y) (!(x) || (y)) //импликация
#define COMPARE(x, y) (((x) > (y)) - ((x) < (y))) //быстрый компаратор целых чисел
#define COMPARE_FLOATS(a, b, epsilon) (fabs(a - b) <= epsilon * fabs(a)) //компаратор чисел с плавающей точкой

#define LENGTH(a) (sizeof(a) / sizeof(*a)) //возвращает длину массива
#define GOOGLE_LENGTH(x) ((sizeof(x)/sizeof(0[x])) / ((size_t)(!(sizeof(x) % sizeof(0[x]))))) //длина по версии гугла

#define SWAP(a, b) do { a ^= b; b ^= a; a ^= b; } while ( 0 )
#define SORT(a, b) do { if ((a) > (b)) SWAP((a), (b)); } while (0)

/* bits */ 

#define BIT(x) (1<<(x))
#define SETBIT(x,p) ((x)|(1<<(p)))
#define SET(d, n, v) do{ size_t i_, n_; for (n_ = (n), i_ = 0; n_ > 0; --n_, ++i_) (d)[i_] = (v); } while(0)
#define ZERO(d, n) SET(d, n, 0) //обнуляет все биты переменной


/* strings */

#define STR(s) (#s)
#define CAT(str1,str2) (str1 "" str2)
#define PRINT_TOKEN(token) printf(#token " is %d", token)


/* debugs */

#define DEBUG

#ifndef DEBUG
#define LOG(x, fmt, ...)
#define TRY(x,s)
#define ASSERT(n)
#define WLOG(x, fmt, ...)
#define WTRY(x,s)
#define WASSERT(n)
#else
#define LOG(x, fmt, ...)    if(x){ printf("%s:%d: " fmt "\n", __FILE__, __LINE__,__VA_ARGS__); }
#define TRY(x,s)            if(!(x)){ printf("%s:%d:%s",__FILE__, __LINE__,s); }
#define ASSERT(n)           if(!(n)) { \
                            printf("%s - Failed ",#n); \
                            printf("On %s ",__DATE__); \
                            printf("At %s ",__TIME__); \
                            printf("In File %s ",__FILE__); \
                            printf("At Line %d\n",__LINE__); \
                            return(-1);}
#define WLOG(x, fmt, ...)    if(x){ wprintf(L"%s:%d: " fmt L"\n", __FILE__, __LINE__,__VA_ARGS__); }
#define WTRY(x,s)            if(!(x)){ wprintf(L"%s:%d:%s",__FILE__, __LINE__,s); }
#define WASSERT(n)           if(!(n)) { \
                            wprintf(L"%s - Failed ",#n); \
                            wprintf(L"On %s ",__DATE__); \
                            wprintf(L"At %s ",__TIME__); \
                            wprintf(L"In File %s ",__FILE__); \
                            wprintf(L"At Line %d\n",__LINE__); \
                            return(-1);}
#endif


/* C in C++ */

#ifdef __cplusplus
#define EXTERN_C_START  extern "C" {
#define EXTERN_C_END    }
#else
#define EXTERN_C_START
#define EXTERN_C_END
#endif
