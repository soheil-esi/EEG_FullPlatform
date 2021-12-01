import abc 


class IServices :

    @classmethod
    def __subclasshook__(cls, subclass):
        return (hasattr(subclass, 'Add') and 
                callable(subclass.Add) or 
                NotImplemented)
