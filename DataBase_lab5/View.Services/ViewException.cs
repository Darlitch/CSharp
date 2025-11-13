namespace View.Services;

public class ViewException(string error, Exception? ex = null) : Exception(error, ex);