import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Dialog, DialogTrigger, DialogContent, DialogTitle, DialogDescription, DialogHeader, DialogFooter, DialogClose } from "@/components/ui/dialog";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Plus, FileText, Trash2 } from "lucide-react";

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
}

export function Bills() {    
    const bills: Bill[] = [
    {
        "id": "1f3e3a1a-8c68-4537-9e4d-56c1f19a8bc9",
        "name": "Electricity Bill",
        "category": "Utilities",
        "status": "Unpaid",
        "amountDue": 120.50,
        "amountPaid": null,
        "createdDate": "2024-08-01T09:30:00",
        "dueDate": "2024-09-01T00:00:00",
        "paidDate": null,
        "deletedDate": null
    },
    {
        "id": "7b9c8277-f8f0-48f2-bc29-d1e38e7d79e5",
        "name": "Internet Bill",
        "category": "Utilities",
        "status": "Paid",
        "amountDue": 75.00,
        "amountPaid": 75.00,
        "createdDate": "2024-08-03T12:00:00",
        "dueDate": "2024-09-03T00:00:00",
        "paidDate": "2024-08-20T11:00:00",
        "deletedDate": null
    },
    {
        "id": "c1b232a4-8275-4fa9-a3df-cb8e18c46c73",
        "name": "Water Bill",
        "category": "Utilities",
        "status": "Unpaid",
        "amountDue": 45.75,
        "amountPaid": null,
        "createdDate": "2024-08-05T10:00:00",
        "dueDate": "2024-09-05T00:00:00",
        "paidDate": null,
        "deletedDate": null
    },
    {
        "id": "39c2a346-5c85-4238-bc46-b122b916e1d8",
        "name": "Rent",
        "category": "Housing",
        "status": "Paid",
        "amountDue": 1500.00,
        "amountPaid": 1500.00,
        "createdDate": "2024-07-25T08:00:00",
        "dueDate": "2024-08-01T00:00:00",
        "paidDate": "2024-07-28T15:00:00",
        "deletedDate": null
    },
    {
        "id": "9984e527-24b6-45b1-9af5-8c053e1b8d26",
        "name": "Car Loan",
        "category": "Loans",
        "status": "Unpaid",
        "amountDue": 320.00,
        "amountPaid": null,
        "createdDate": "2024-08-10T13:00:00",
        "dueDate": "2024-09-10T00:00:00",
        "paidDate": null,
        "deletedDate": null
    },
    {
        "id": "c68a8932-fb07-4997-bd23-13387d41e132",
        "name": "Gym Membership",
        "category": "Health & Fitness",
        "status": "Paid",
        "amountDue": 45.00,
        "amountPaid": 45.00,
        "createdDate": "2024-08-01T09:00:00",
        "dueDate": "2024-09-01T00:00:00",
        "paidDate": "2024-08-15T10:00:00",
        "deletedDate": null
    },
    {
        "id": "df4c2bc2-624f-4b1f-9dd9-8a5e1cf2c264",
        "name": "Credit Card Bill",
        "category": "Credit Card",
        "status": "Unpaid",
        "amountDue": 600.00,
        "amountPaid": null,
        "createdDate": "2024-08-08T09:00:00",
        "dueDate": "2024-09-08T00:00:00",
        "paidDate": null,
        "deletedDate": null
    },
    {
        "id": "f84d7cb5-24ad-4a94-9376-4c791b8b9634",
        "name": "Phone Bill",
        "category": "Utilities",
        "status": "Paid",
        "amountDue": 60.00,
        "amountPaid": 60.00,
        "createdDate": "2024-08-01T11:00:00",
        "dueDate": "2024-09-01T00:00:00",
        "paidDate": "2024-08-10T08:00:00",
        "deletedDate": null
    },
    {
        "id": "1b9b8b44-cc10-46df-a4d4-56425b5b25ef",
        "name": "Netflix Subscription",
        "category": "Entertainment",
        "status": "Paid",
        "amountDue": 15.99,
        "amountPaid": 15.99,
        "createdDate": "2024-08-05T12:30:00",
        "dueDate": "2024-09-05T00:00:00",
        "paidDate": "2024-08-06T14:00:00",
        "deletedDate": null
    },
    {
        "id": "c7ad6e6d-ccbb-4a0d-a4bc-416f25e0fc68",
        "name": "Student Loan",
        "category": "Loans",
        "status": "Unpaid",
        "amountDue": 250.00,
        "amountPaid": null,
        "createdDate": "2024-08-15T14:00:00",
        "dueDate": "2024-09-15T00:00:00",
        "paidDate": null,
        "deletedDate": null
    }
    ];

    return (
        <div className="mt-4 mx-2 p-4 border border-zinc-700 rounded-xl flex flex-col">
            <div className="flex flex-row justify-between mt-2 mx-4 mb-4">
                <h1 className="font-bold text-2xl">Bills</h1>
                <Button className="flex items-center gap-2">
                    <Plus />
                    Add bill
                </Button>
            </div>
            <Table>
                <TableHeader>
                    <TableRow>
                        <TableHead>Name</TableHead>
                        <TableHead>Category</TableHead>
                        <TableHead>Status</TableHead>
                        <TableHead>Amount Due</TableHead>
                        <TableHead>Actions</TableHead>
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {bills.map((bill) => {
                        return (
                            <TableRow key={bill.id}>
                                <TableCell className="font-semibold">{bill.name}</TableCell>
                                <TableCell>{bill.category}</TableCell>
                                <TableCell>
                                    <Badge variant={bill.status === 'Paid' ? 'default' : 'outline'}>
                                        {bill.status}
                                    </Badge>
                                </TableCell>
                                <TableCell className="font-semibold">$ {bill.amountDue.toFixed(2)}</TableCell>
                                <TableCell className="flex flex-row gap-2">
                                    <Button variant="outline" className="flex items-center gap-2">
                                        <FileText /> Details
                                    </Button>
                                    <Dialog>
                                        <DialogTrigger>
                                            <Button variant="destructive" className="flex items-center gap-2">
                                                <Trash2 /> Delete
                                            </Button>
                                        </DialogTrigger>
                                        <DialogContent>
                                            <DialogHeader>
                                                <DialogTitle>Are you absolutely sure?</DialogTitle>
                                                <DialogDescription>
                                                    This action cannot be undone. This will permanently delete your bill
                                                    and remove all its data.
                                                </DialogDescription>
                                            </DialogHeader>
                                            <DialogFooter className="sm:justify-start">
                                                <DialogClose asChild>
                                                    <Button type="button" variant="destructive">
                                                        Delete
                                                    </Button>
                                                </DialogClose>
                                            </DialogFooter>
                                        </DialogContent>
                                    </Dialog>                            
                                </TableCell>
                            </TableRow>
                        );
                    })}
                </TableBody>
            </Table>
        </div>
    )
}