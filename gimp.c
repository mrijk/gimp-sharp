// gimp.c
//

#include <stdarg.h>
#include <stdlib.h>

#include <libgimp/gimp.h>

#ifndef STDCALL
#define STDCALL __attribute__((stdcall))
#endif

static GimpPlugInInfo _wrap, _info;

static void wrap_init(void)
{
  ((void (STDCALL *)()) _info.init_proc)();
}

static void wrap_query(void)
{
  ((void (STDCALL *)()) _info.query_proc)();
}

static void wrap_quit(void)
{
  ((void (STDCALL *)()) _info.quit_proc)();
}

static void wrap_run(const char *name, int n_params, const GimpParam *param, 
		     int *n_return_vals, GimpParam **return_vals)
{
#ifndef _OLD_
  ((void (STDCALL *)(const char*, int, const GimpParam*, int*, GimpParam**)) _info.run_proc)(name, n_params, param, n_return_vals, return_vals);
#else
  static GimpParam dummy[2];

  ((void (STDCALL *)(int, const GimpParam*)) _info.run_proc)(n_params, param);
  *n_return_vals = 0;
  return_vals = NULL;
#endif
}

int fnInitGimp(GimpPlugInInfo *info, int argc, char *args[])
{
  _info = *info;
  
  if (info->init_proc)
    _wrap.init_proc = wrap_init;
  if (info->query_proc)
    _wrap.query_proc = wrap_query;
  if (info->quit_proc)
    _wrap.quit_proc = wrap_quit;
  if (info->run_proc)
    _wrap.run_proc = wrap_run;

  return gimp_main (&_wrap, argc, args);
}

gboolean wrapper_set_data(const gchar  *identifier,
			  const guint8 *data,
			  gint          bytes)
{
  return gimp_set_data(identifier, data, bytes);
}

gboolean wrapper_get_data(const gchar  *identifier,
			  guint8 *data)
{
  return gimp_get_data(identifier, data);
}

int wrapper_get_data_size(const char *identifier)
{
  return gimp_get_data_size(identifier);
}			  

int gimp_main_wrapper(GimpPlugInInfo *info)
{
	info->query_proc();
	return info == &_wrap;
}

static int n_return_vals;
static GimpParam *return_vals;


// This is another example of an exported function.
int fnGimp2(void (*func)(const char*))
{
	((void (STDCALL *)(const char*)) func)("foo");
	return 42;
}

int fnGimp3(char *fmt, ...)
{
	int count = 0;
	char *val;

	va_list	ap;

	va_start(ap, fmt);
	while ((val = va_arg(ap, char*)) != NULL)
		{
		count++;
		}

	va_end(ap);

	return count;
}

// This is an example of an exported function.
int fnGimp()
{
	return fnGimp3("format", "aap", "noot", NULL);
}
