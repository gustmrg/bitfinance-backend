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
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogTrigger,
  DialogContent,
  DialogTitle,
  DialogDescription,
  DialogHeader,
  DialogFooter,
  DialogClose,
} from "@/components/ui/dialog";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  Plus,
  FileText,
  Trash2,
  PlusCircle,
  MoreHorizontal,
} from "lucide-react";
import { useState } from "react";
import { Link } from "react-router-dom";

type Bill = {
  id: string;
  name: string;
  category: string;
  status: string;
  amountDue: number;
  amountPaid?: number | null;
  createdDate: string;
  dueDate: string;
  paidDate?: string | null;
  deletedDate?: string | null;
};

export function Bills() {
  const [bills, setBills] = useState<Bill[]>([
    {
      id: "1f3e3a1a-8c68-4537-9e4d-56c1f19a8bc9",
      name: "Electricity Bill",
      category: "Utilities",
      status: "Unpaid",
      amountDue: 120.5,
      amountPaid: null,
      createdDate: "2024-08-01T09:30:00",
      dueDate: "2024-09-01T00:00:00",
      paidDate: null,
      deletedDate: null,
    },
    {
      id: "7b9c8277-f8f0-48f2-bc29-d1e38e7d79e5",
      name: "Internet Bill",
      category: "Utilities",
      status: "Paid",
      amountDue: 75.0,
      amountPaid: 75.0,
      createdDate: "2024-08-03T12:00:00",
      dueDate: "2024-09-03T00:00:00",
      paidDate: "2024-08-20T11:00:00",
      deletedDate: null,
    },
    {
      id: "c1b232a4-8275-4fa9-a3df-cb8e18c46c73",
      name: "Water Bill",
      category: "Utilities",
      status: "Unpaid",
      amountDue: 45.75,
      amountPaid: null,
      createdDate: "2024-08-05T10:00:00",
      dueDate: "2024-09-05T00:00:00",
      paidDate: null,
      deletedDate: null,
    },
    {
      id: "39c2a346-5c85-4238-bc46-b122b916e1d8",
      name: "Rent",
      category: "Housing",
      status: "Paid",
      amountDue: 1500.0,
      amountPaid: 1500.0,
      createdDate: "2024-07-25T08:00:00",
      dueDate: "2024-08-01T00:00:00",
      paidDate: "2024-07-28T15:00:00",
      deletedDate: null,
    },
    {
      id: "9984e527-24b6-45b1-9af5-8c053e1b8d26",
      name: "Car Loan",
      category: "Loans",
      status: "Unpaid",
      amountDue: 320.0,
      amountPaid: null,
      createdDate: "2024-08-10T13:00:00",
      dueDate: "2024-09-10T00:00:00",
      paidDate: null,
      deletedDate: null,
    },
    {
      id: "c68a8932-fb07-4997-bd23-13387d41e132",
      name: "Gym Membership",
      category: "Health & Fitness",
      status: "Paid",
      amountDue: 45.0,
      amountPaid: 45.0,
      createdDate: "2024-08-01T09:00:00",
      dueDate: "2024-09-01T00:00:00",
      paidDate: "2024-08-15T10:00:00",
      deletedDate: null,
    },
    {
      id: "df4c2bc2-624f-4b1f-9dd9-8a5e1cf2c264",
      name: "Credit Card Bill",
      category: "Credit Card",
      status: "Unpaid",
      amountDue: 600.0,
      amountPaid: null,
      createdDate: "2024-08-08T09:00:00",
      dueDate: "2024-09-08T00:00:00",
      paidDate: null,
      deletedDate: null,
    },
    {
      id: "f84d7cb5-24ad-4a94-9376-4c791b8b9634",
      name: "Phone Bill",
      category: "Utilities",
      status: "Paid",
      amountDue: 60.0,
      amountPaid: 60.0,
      createdDate: "2024-08-01T11:00:00",
      dueDate: "2024-09-01T00:00:00",
      paidDate: "2024-08-10T08:00:00",
      deletedDate: null,
    },
    {
      id: "1b9b8b44-cc10-46df-a4d4-56425b5b25ef",
      name: "Netflix Subscription",
      category: "Entertainment",
      status: "Paid",
      amountDue: 15.99,
      amountPaid: 15.99,
      createdDate: "2024-08-05T12:30:00",
      dueDate: "2024-09-05T00:00:00",
      paidDate: "2024-08-06T14:00:00",
      deletedDate: null,
    },
    {
      id: "c7ad6e6d-ccbb-4a0d-a4bc-416f25e0fc68",
      name: "Student Loan",
      category: "Loans",
      status: "Unpaid",
      amountDue: 250.0,
      amountPaid: null,
      createdDate: "2024-08-15T14:00:00",
      dueDate: "2024-09-15T00:00:00",
      paidDate: null,
      deletedDate: null,
    },
  ]);

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

  return (
    <>
      <div className="flex items-center space-x-4 my-4 justify-between">
        <h1 className="text-3xl font-semibold">Bills</h1>
        <Dialog>
          <DialogTrigger asChild>
            <Button>
              <PlusCircle className="mr-2 h-4 w-4" />
              Add Bill
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>Add New Bill</DialogTitle>
            </DialogHeader>
            <div className="grid gap-4 py-4">
              <div className="grid grid-cols-4 items-center gap-4">
                <Label htmlFor="description" className="text-right">
                  Description
                </Label>
                <Input
                  id="description"
                  className="col-span-3"
                  value={""}
                  onChange={() => {}}
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
                  value={""}
                  onChange={() => {}}
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
                  value={""}
                  onChange={() => {}}
                />
              </div>
              <div className="grid grid-cols-4 items-center gap-4">
                <Label htmlFor="status" className="text-right">
                  Status
                </Label>
                <Select value={""} onValueChange={() => {}}>
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
            </div>
            <DialogFooter>
              <Button onClick={() => {}}>Add Bill</Button>
            </DialogFooter>
          </DialogContent>
        </Dialog>
      </div>
      <div className="rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Name</TableHead>
              <TableHead>Category</TableHead>
              <TableHead>Status</TableHead>
              <TableHead className="text-right">Amount</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {bills.map((bill) => {
              return (
                <TableRow key={bill.id}>
                  <TableCell className="font-semibold">{bill.name}</TableCell>
                  <TableCell>{bill.category}</TableCell>
                  <TableCell>
                    <Badge
                      variant={bill.status === "Paid" ? "default" : "outline"}
                    >
                      {bill.status}
                    </Badge>
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
                            <DialogHeader>
                              <DialogTitle>Transaction Details</DialogTitle>
                            </DialogHeader>
                            <div className="mt-4">
                              <p>
                                <strong>Date:</strong> {bill.createdDate}
                              </p>
                              <p>
                                <strong>Description:</strong> {bill.name}
                              </p>
                              <p>
                                <strong>Amount:</strong> $
                                {bill.amountDue.toFixed(2)}
                              </p>
                              <p>
                                <strong>Status:</strong> {bill.status}
                              </p>
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
                                permanently delete the transaction.
                              </AlertDialogDescription>
                            </AlertDialogHeader>
                            <AlertDialogFooter>
                              <AlertDialogCancel>Cancel</AlertDialogCancel>
                              <AlertDialogAction onClick={() => {}}>
                                Delete
                              </AlertDialogAction>
                            </AlertDialogFooter>
                          </AlertDialogContent>
                        </AlertDialog>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </TableCell>
                </TableRow>
              );
            })}
          </TableBody>
        </Table>
      </div>
    </>
  );
}
