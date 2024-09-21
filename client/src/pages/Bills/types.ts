export type Bill = {
  id: string;
  description: string;
  category: string;
  status: string;
  amountDue: number;
  amountPaid?: number | null;
  createdDate: string;
  dueDate: string;
  paidDate?: string | null;
  deletedDate?: string | null;
  notes?: string;
};
