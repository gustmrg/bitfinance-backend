import { useState } from "react";
import { Button } from "@/components/ui/button";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog";
import {
  DropdownMenu,
  DropdownMenuCheckboxItem,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
  MoreHorizontal,
  FileText,
  Trash2,
  AlertCircle,
  CheckCircle2,
  XCircle,
  PlusCircle,
  Paperclip,
  ListFilter,
} from "lucide-react";
import { Badge } from "@/components/ui/badge";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { dateFormatter, priceFormatter } from "@/utils/formatter";

type Bill = {
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
};

// Mock data for bills
const initialBills: Bill[] = [
  {
    id: "1f3e3a1a-8c68-4537-9e4d-56c1f19a8bc9",
    description: "Electricity Bill",
    category: "Utilities",
    status: "due",
    amountDue: 120.5,
    amountPaid: null,
    createdDate: "2024-08-01T09:30:00",
    dueDate: "2024-09-01T00:00:00",
    paidDate: null,
    deletedDate: null,
  },
  {
    id: "7b9c8277-f8f0-48f2-bc29-d1e38e7d79e5",
    description: "Internet Bill",
    category: "Utilities",
    status: "paid",
    amountDue: 75.0,
    amountPaid: 75.0,
    createdDate: "2024-08-03T12:00:00",
    dueDate: "2024-09-03T00:00:00",
    paidDate: "2024-08-20T11:00:00",
    deletedDate: null,
  },
  {
    id: "c1b232a4-8275-4fa9-a3df-cb8e18c46c73",
    description: "Water Bill",
    category: "Utilities",
    status: "overdue",
    amountDue: 45.75,
    amountPaid: null,
    createdDate: "2024-08-05T10:00:00",
    dueDate: "2024-09-05T00:00:00",
    paidDate: null,
    deletedDate: null,
  },
  {
    id: "39c2a346-5c85-4238-bc46-b122b916e1d8",
    description: "Rent",
    category: "Housing",
    status: "paid",
    amountDue: 1500.0,
    amountPaid: 1500.0,
    createdDate: "2024-07-25T08:00:00",
    dueDate: "2024-08-01T00:00:00",
    paidDate: "2024-07-28T15:00:00",
    deletedDate: null,
  },
  {
    id: "9984e527-24b6-45b1-9af5-8c053e1b8d26",
    description: "Car Loan",
    category: "Loans",
    status: "cancelled",
    amountDue: 320.0,
    amountPaid: null,
    createdDate: "2024-08-10T13:00:00",
    dueDate: "2024-09-10T00:00:00",
    paidDate: null,
    deletedDate: null,
  },
  {
    id: "c68a8932-fb07-4997-bd23-13387d41e132",
    description: "Gym Membership",
    category: "Health & Fitness",
    status: "paid",
    amountDue: 45.0,
    amountPaid: 45.0,
    createdDate: "2024-08-01T09:00:00",
    dueDate: "2024-09-01T00:00:00",
    paidDate: "2024-08-15T10:00:00",
    deletedDate: null,
  },
  {
    id: "df4c2bc2-624f-4b1f-9dd9-8a5e1cf2c264",
    description: "Credit Card Bill",
    category: "Credit Card",
    status: "due",
    amountDue: 600.0,
    amountPaid: null,
    createdDate: "2024-08-08T09:00:00",
    dueDate: "2024-09-08T00:00:00",
    paidDate: null,
    deletedDate: null,
  },
  {
    id: "f84d7cb5-24ad-4a94-9376-4c791b8b9634",
    description: "Phone Bill",
    category: "Utilities",
    status: "paid",
    amountDue: 60.0,
    amountPaid: 60.0,
    createdDate: "2024-08-01T11:00:00",
    dueDate: "2024-09-01T00:00:00",
    paidDate: "2024-08-10T08:00:00",
    deletedDate: null,
  },
  {
    id: "1b9b8b44-cc10-46df-a4d4-56425b5b25ef",
    description: "Netflix Subscription",
    category: "Entertainment",
    status: "paid",
    amountDue: 15.99,
    amountPaid: 15.99,
    createdDate: "2024-08-05T12:30:00",
    dueDate: "2024-09-05T00:00:00",
    paidDate: "2024-08-06T14:00:00",
    deletedDate: null,
  },
  {
    id: "c7ad6e6d-ccbb-4a0d-a4bc-416f25e0fc68",
    description: "Student Loan",
    category: "Loans",
    status: "overdue",
    amountDue: 250.0,
    amountPaid: null,
    createdDate: "2024-08-15T14:00:00",
    dueDate: "2024-09-15T00:00:00",
    paidDate: null,
    deletedDate: null,
  },
];

export function Bills() {
  const [bills, setBills] = useState<Bill[]>(initialBills);
  const [newBill, setNewBill] = useState<Bill>({
    id: "",
    description: "",
    category: "",
    status: "completed",
    amountDue: 0,
    createdDate: new Date().toISOString(),
    dueDate: new Date().toISOString(),
  });

  function handleCancel(id: string) {
    const updatedBills = bills.map((bill) =>
      bill.id === id
        ? {
            ...bill,
            status: "Cancelled",
          }
        : bill,
    );

    setBills(updatedBills);
  }

  const handleAddBill = () => {
    const bill: Bill = {
      id: "1f3e3a1a-8c68-4537-9e4d-56c1f19a8bc9",
      description: newBill.description,
      category: newBill.category,
      status: newBill.status,
      amountDue: newBill.amountDue,
      amountPaid: newBill.amountPaid,
      createdDate: new Date().toISOString(),
      dueDate: new Date().toISOString(),
    };
    setBills([...bills, bill]);
    setNewBill({
      id: "",
      description: "",
      category: "",
      status: "completed",
      amountDue: 0,
      createdDate: new Date().toISOString(),
      dueDate: new Date().toISOString(),
    });
  };

  const getStatusBadge = (status: string) => {
    switch (status) {
      case "paid":
        return <Badge variant="success">Paid</Badge>;
      case "due":
        return <Badge variant="warning">Due</Badge>;
      case "overdue":
        return <Badge variant="destructive">Overdue</Badge>;
      case "cancelled":
        return <Badge variant="secondary">Cancelled</Badge>;
      default:
        return <Badge variant="secondary">Created</Badge>;
    }
  };

  return (
    <>
      <div className="flex items-center space-x-4 my-4 justify-between">
        <h1 className="text-3xl font-semibold">Bills</h1>
        <div className="ml-auto flex items-center gap-2">
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <Button variant="outline" size="sm" className="h-8 gap-1">
                <ListFilter className="h-3.5 w-3.5" />
                <span className="sr-only sm:not-sr-only sm:whitespace-nowrap">
                  Filter
                </span>
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent align="end">
              <DropdownMenuLabel>Filter by</DropdownMenuLabel>
              <DropdownMenuSeparator />
              <DropdownMenuCheckboxItem checked>All</DropdownMenuCheckboxItem>
              <DropdownMenuCheckboxItem>Completed</DropdownMenuCheckboxItem>
              <DropdownMenuCheckboxItem>Pending</DropdownMenuCheckboxItem>
              <DropdownMenuCheckboxItem>Failed</DropdownMenuCheckboxItem>
            </DropdownMenuContent>
          </DropdownMenu>
          <Dialog>
            <DialogTrigger asChild>
              <Button size="sm" className="h-8 gap-1">
                <PlusCircle className="h-3.5 w-3.5" />
                <span className="sr-only sm:not-sr-only sm:whitespace-nowrap">
                  Add Bill
                </span>
              </Button>
            </DialogTrigger>
            <DialogContent>
              <DialogHeader>
                <DialogTitle>Add New Bill</DialogTitle>
              </DialogHeader>
              {/* <div className="grid gap-4 py-4">
                <div className="grid grid-cols-4 items-center gap-4">
                  <Label htmlFor="name" className="text-right">
                    Name
                  </Label>
                  <Input
                    id="name"
                    className="col-span-3"
                    value={newBill.name}
                    onChange={(e) =>
                      setNewBill({
                        ...newBill,
                        name: e.target.value,
                      })
                    }
                  />
                </div>
                <div className="grid grid-cols-4 items-center gap-4">
                  <Label htmlFor="date" className="text-right">
                    Date
                  </Label>
                  <Input
                    id="date"
                    type="date"
                    className="col-span-3"
                    value={newBill.dueDate}
                    onChange={(e) =>
                      setNewBill({
                        ...newBill,
                        dueDate: e.target.value,
                      })
                    }
                  />
                </div>
                <div className="grid grid-cols-4 items-center gap-4">
                  <Label htmlFor="amount" className="text-right">
                    Amount
                  </Label>
                  <Input
                    id="amount"
                    type="number"
                    className="col-span-3"
                    value={newBill.amountDue}
                    onChange={(e) =>
                      setNewBill({
                        ...newBill,
                        amountDue: e.target.value,
                      })
                    }
                  />
                </div>
                <div className="grid grid-cols-4 items-center gap-4">
                  <Label htmlFor="status" className="text-right">
                    Status
                  </Label>
                  <Select
                    value={newBill.status}
                    onValueChange={(value) =>
                      setNewBill({ ...newBill, status: value })
                    }
                  >
                    <SelectTrigger className="col-span-3">
                      <SelectValue placeholder="Select status" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="completed">Completed</SelectItem>
                      <SelectItem value="pending">Pending</SelectItem>
                      <SelectItem value="failed">Failed</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </div> */}
              <DialogFooter>
                <Button onClick={handleAddBill}>Add Bill</Button>
              </DialogFooter>
            </DialogContent>
          </Dialog>
        </div>
      </div>
      <div className="rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Description</TableHead>
              <TableHead>Category</TableHead>
              <TableHead>Status</TableHead>
              <TableHead>Date</TableHead>
              <TableHead className="text-right">Amount</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {bills.map((bill) => (
              <TableRow key={bill.id}>
                <TableCell className="font-semibold">
                  {bill.description}
                </TableCell>
                <TableCell>{bill.category}</TableCell>
                <TableCell>{getStatusBadge(bill.status)}</TableCell>
                <TableCell>
                  {dateFormatter.format(new Date(bill.createdDate))}
                </TableCell>
                <TableCell className="text-right">
                  $ {bill.amountDue.toFixed(2)}
                </TableCell>
                <TableCell className="text-right">
                  <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                      <Button variant="ghost" className="h-8 w-8 p-0">
                        <span className="sr-only">Open menu</span>
                        <MoreHorizontal className="h-4 w-4" />
                      </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                      <Dialog>
                        <DialogTrigger asChild>
                          <DropdownMenuItem
                            onSelect={(e) => e.preventDefault()}
                          >
                            <FileText className="mr-2 h-4 w-4" />
                            <span>Details</span>
                          </DropdownMenuItem>
                        </DialogTrigger>
                        <DialogContent>
                          <DialogHeader className="px-4 sm:px-0">
                            <DialogTitle className="text-base font-semibold leading-7 text-gray-900">
                              Bill Details
                            </DialogTitle>
                            <p className="mt-1 max-w-2xl text-sm leading-6 text-gray-500">
                              Here you can view detailed information about the
                              selected bill, including its status, due date, and
                              any associated receipts.
                            </p>
                          </DialogHeader>
                          <div className="mt-6 border-t border-gray-100">
                            <dl className="divide-y divide-gray-100">
                              <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
                                <dt className="text-sm font-medium leading-6 text-gray-900">
                                  Description
                                </dt>
                                <dd className="mt-1 text-sm leading-6 text-gray-700 sm:col-span-2 sm:mt-0">
                                  {bill.description}
                                </dd>
                              </div>
                              <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
                                <dt className="text-sm font-medium leading-6 text-gray-900">
                                  Category
                                </dt>
                                <dd className="mt-1 text-sm leading-6 text-gray-700 sm:col-span-2 sm:mt-0">
                                  {bill.category}
                                </dd>
                              </div>
                              <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
                                <dt className="text-sm font-medium leading-6 text-gray-900">
                                  Status
                                </dt>
                                <dd className="mt-1 text-sm leading-6 text-gray-700 sm:col-span-2 sm:mt-0">
                                  {getStatusBadge(bill.status)}
                                </dd>
                              </div>
                              {bill.status === "paid" ? (
                                <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
                                  <dt className="text-sm font-medium leading-6 text-gray-900">
                                    Paid Date
                                  </dt>
                                  <dd className="mt-1 text-sm leading-6 text-gray-700 sm:col-span-2 sm:mt-0">
                                    {bill.paidDate}
                                  </dd>
                                </div>
                              ) : (
                                <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
                                  <dt className="text-sm font-medium leading-6 text-gray-900">
                                    Due Date
                                  </dt>
                                  <dd className="mt-1 text-sm leading-6 text-gray-700 sm:col-span-2 sm:mt-0">
                                    {bill.dueDate}
                                  </dd>
                                </div>
                              )}

                              {bill.status === "paid" ? (
                                <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
                                  <dt className="text-sm font-medium leading-6 text-gray-900">
                                    Amount Paid
                                  </dt>
                                  <dd className="mt-1 text-sm leading-6 text-gray-700 sm:col-span-2 sm:mt-0">
                                    {`$${bill.amountPaid?.toFixed(2)}`}
                                  </dd>
                                </div>
                              ) : (
                                <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
                                  <dt className="text-sm font-medium leading-6 text-gray-900">
                                    Amount Due
                                  </dt>
                                  <dd className="mt-1 text-sm leading-6 text-gray-700 sm:col-span-2 sm:mt-0">
                                    {`$${bill.amountDue.toFixed(2)}`}
                                  </dd>
                                </div>
                              )}

                              <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
                                <dt className="text-sm font-medium leading-6 text-gray-900">
                                  Notes
                                </dt>
                                <dd className="mt-1 text-sm leading-6 text-gray-700 sm:col-span-2 sm:mt-0">
                                  Fugiat ipsum ipsum deserunt culpa aute sint do
                                  nostrud anim incididunt cillum culpa
                                  consequat.
                                </dd>
                              </div>
                              <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
                                <dt className="text-sm font-medium leading-6 text-gray-900">
                                  Attachments
                                </dt>
                                <dd className="mt-2 text-sm text-gray-900 sm:col-span-2 sm:mt-0">
                                  <ul
                                    role="list"
                                    className="divide-y divide-gray-100 rounded-md border border-gray-200"
                                  >
                                    <li className="flex items-center justify-between py-4 pl-4 pr-5 text-sm leading-6">
                                      <div className="flex w-0 flex-1 items-center">
                                        <Paperclip
                                          aria-hidden="true"
                                          className="h-5 w-5 flex-shrink-0 text-gray-400"
                                        />
                                        <div className="ml-4 flex min-w-0 flex-1 gap-2">
                                          <span className="truncate font-medium">
                                            resume_back_end_developer.pdf
                                          </span>
                                        </div>
                                      </div>
                                      <div className="ml-4 flex-shrink-0">
                                        <a
                                          href="#"
                                          className="font-medium text-indigo-600 hover:text-indigo-500"
                                        >
                                          Download
                                        </a>
                                      </div>
                                    </li>
                                    <li className="flex items-center justify-between py-4 pl-4 pr-5 text-sm leading-6">
                                      <div className="flex w-0 flex-1 items-center">
                                        <Paperclip
                                          aria-hidden="true"
                                          className="h-5 w-5 flex-shrink-0 text-gray-400"
                                        />
                                        <div className="ml-4 flex min-w-0 flex-1 gap-2">
                                          <span className="truncate font-medium">
                                            coverletter_back_end_developer.pdf
                                          </span>
                                        </div>
                                      </div>
                                      <div className="ml-4 flex-shrink-0">
                                        <a
                                          href="#"
                                          className="font-medium text-indigo-600 hover:text-indigo-500"
                                        >
                                          Download
                                        </a>
                                      </div>
                                    </li>
                                  </ul>
                                </dd>
                              </div>
                            </dl>
                          </div>
                        </DialogContent>
                      </Dialog>
                      <AlertDialog>
                        <AlertDialogTrigger asChild>
                          <DropdownMenuItem
                            onSelect={(e) => e.preventDefault()}
                            className="text-red-600"
                          >
                            <Trash2 className="mr-2 h-4 w-4" />
                            <span>Delete</span>
                          </DropdownMenuItem>
                        </AlertDialogTrigger>
                        <AlertDialogContent>
                          <AlertDialogHeader>
                            <AlertDialogTitle>Are you sure?</AlertDialogTitle>
                            <AlertDialogDescription>
                              This action cannot be undone. This will
                              permanently delete the record.
                            </AlertDialogDescription>
                          </AlertDialogHeader>
                          <AlertDialogFooter>
                            <AlertDialogCancel>Cancelar</AlertDialogCancel>
                            <AlertDialogAction
                              className="bg-red-600 hover:bg-red-700"
                              onClick={() => handleCancel(bill.id)}
                            >
                              Delete
                            </AlertDialogAction>
                          </AlertDialogFooter>
                        </AlertDialogContent>
                      </AlertDialog>
                    </DropdownMenuContent>
                  </DropdownMenu>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
    </>
  );
}
