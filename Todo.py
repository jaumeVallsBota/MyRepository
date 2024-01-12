"""
Created on Fri Jan 12 09:32:07 2024

@author: jeva
"""
import tkinter as tk

class TaskList(tk.Tk):
    def __init__(self, tasks=None):
        super().__init__()
        
        if not tasks:
            self.tasks = []
        else:
            self.tasks = tasks
            
        self.title("Task List")
        
        instructions = tk.Label(self, text="Enter items into input field", bg="yellow", pady=10)
        self.tasks.append(instructions)
        
        for task in self.tasks:
            task.pack(fill=tk.X)
        self.new_task = tk.Text(self, height = 20)
        self.new_task.pack()
        self.new_task.focus_set()
        
        #When user clicks Enter, the function add_task will be called
        self.bind("<Return>", self.add_Task)
        
        self.colors = [{
                "bg":"#E15F14"
            },
            {
                "bg":"green"}]
    def add_Task (self, event= None):
        task_text = self.new_task.get(1.0, tk.END).strip()
        
        task_label = tk.Label(self, text = task_text, pady=10)
        _, task_color = divmod(len(self.tasks), 2)
        task_label.configure(bg=self.colors[task_color]["bg"])
        task_label.pack(fill=tk.X)
        self.tasks.append(task_label)
        self.new_task.delete(1.0, tk.END)

if __name__ == "__main__":
    task_List = TaskList()
    task_List.mainloop()
