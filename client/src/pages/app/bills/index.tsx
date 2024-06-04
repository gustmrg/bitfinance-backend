import { Button } from "@/components/ui/button";
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuLabel, DropdownMenuSeparator, DropdownMenuTrigger } from "@/components/ui/dropdown-menu";
import { Input } from "@/components/ui/input";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { MoreHorizontal, PlusCircle, Search, X } from "lucide-react";

interface Bill {
    id: number
    name: string;
    status: 'pending' | 'paid' | 'due' | 'overdue';
    amountDue: number;
    amountPaid?: number;
    dueDate: string;
    paidDate?: string;
}

export function Bills() {
    const bills: Bill[] = [
        {
            id: 1,
            name: 'Electricity Bill',
            status: 'pending',
            amountDue: 75.50,
            dueDate: '2023-06-22T00:00:00Z'
        },
        {
            id: 2,
            name: 'Water Bill',
            status: 'pending',
            amountDue: 75.50,
            dueDate: '2023-06-22T00:00:00Z'
        },
        {
            id: 3,
            name: 'Mortgage',
            status: 'due',
            amountDue: 75.50,
            dueDate: '2023-06-22T00:00:00Z'
        },
        {
            id: 4,
            name: 'Car Insurance',
            status: 'paid',
            amountDue: 75.50,
            dueDate: '2023-06-22T00:00:00Z'
        },
        {
            id: 5,
            name: 'Internet bill',
            status: 'overdue',
            amountDue: 75.50,
            dueDate: '2023-06-22T00:00:00Z'
        },
    ]

    return (
        <>
            <div className="mx-auto grid w-full gap-2">
                <h1 className="text-3xl font-semibold md:text-2xl">Bills</h1>
            </div>
            {bills.length === 0 ? (
                <div
                className="flex flex-1 items-center justify-center rounded-lg border border-dashed shadow-sm" x-chunk="dashboard-02-chunk-1"
                    >
                    <div className="flex flex-col items-center gap-1 text-center">
                        <h3 className="text-2xl font-bold tracking-tight">
                            You have no bills
                        </h3>
                        <p className="text-sm text-muted-foreground">
                            You can start tracking your expenses as soon as you add a bill.
                        </p>
                        <Button className="mt-4">Add Bill</Button>
                    </div>
                </div>
            ) : (
                <div className="flex flex-col rounded-lg border border-solid p-4 space-y-1.5 gap-4">
                    <form className="flex items-center gap-2">
                        <span className="text-sm font-semibold">Filters:</span>
                        <Input placeholder="Id" className="h-8 w-auto" />
                        <Input placeholder="Name" className="h-8 w-[320px]" />
                        <Select defaultValue="all">
                            <SelectTrigger className="h-8 w-[180px]">
                                <SelectValue />
                            </SelectTrigger>
                            <SelectContent>
                                <SelectItem value="all">All</SelectItem>
                                <SelectItem value="due">Due</SelectItem>
                                <SelectItem value="paid">Paid</SelectItem>
                                <SelectItem value="pending">Pending</SelectItem>
                                <SelectItem value="overdue">Overdue</SelectItem>
                                <SelectItem value="cancelled">Cancelled</SelectItem>
                            </SelectContent>
                        </Select>

                        <Button type="submit" variant="outline" className="gap-1">
                            <Search className="h-4 w-4" />
                            <span className="sr-only sm:not-sr-only sm:whitespace-nowrap">
                                Filter results
                            </span>
                        </Button>

                        <Button type="button" variant="outline" className="gap-1">
                            <X className="h-4 w-4" />
                            <span className="sr-only sm:not-sr-only sm:whitespace-nowrap">
                                Clear filters
                            </span>
                        </Button>
                        <Button type="button" className="gap-1">
                            <PlusCircle className="h-4 w-4" />
                            <span className="sr-only sm:not-sr-only sm:whitespace-nowrap">
                                Add Product
                            </span>
                        </Button>
                    </form>
                    <div>
                        <Table>
                            <TableHeader>
                                <TableRow>
                                    <TableHead>Id</TableHead>
                                    <TableHead className="hidden sm:table-cell">Name</TableHead>
                                    <TableHead className="hidden sm:table-cell">Status</TableHead>
                                    <TableHead className="hidden md:table-cell">Due Date</TableHead>
                                    <TableHead className="hidden md:table-cell">Amount Due</TableHead>
                                    <TableHead></TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                            {bills.map((bill) => {
                                return (
                                    <TableRow key={bill.id}>
                                        <TableCell>
                                            <div className="font-mono font-medium text-xs">{bill.id}</div>
                                        </TableCell>
                                        <TableCell className="hidden sm:table-cell font-medium">
                                            {bill.name}
                                        </TableCell>
                                        <TableCell className="hidden sm:table-cell">
                                            <div className="flex items-center gap-2">
                                                <span className="h-2 w-2 bg-slate-400 rounded-full" />
                                                <span className="font-medium text-foreground">{bill.status}</span>
                                            </div>
                                        </TableCell>
                                        <TableCell className="hidden md:table-cell">
                                            {bill.dueDate}
                                        </TableCell>
                                        <TableCell className="font-medium">$ {bill.amountDue.toFixed(2)}</TableCell>
                                        <TableCell>
                                            <DropdownMenu>
                                                <DropdownMenuTrigger asChild>
                                                <Button
                                                    aria-haspopup="true"
                                                    size="icon"
                                                    variant="ghost"
                                                >
                                                    <MoreHorizontal className="h-4 w-4" />
                                                    <span className="sr-only">Toggle menu</span>
                                                </Button>
                                                </DropdownMenuTrigger>
                                                <DropdownMenuContent align="end">
                                                <DropdownMenuLabel>Actions</DropdownMenuLabel>
                                                <DropdownMenuItem>Mark as paid</DropdownMenuItem>
                                                <DropdownMenuSeparator></DropdownMenuSeparator>
                                                <DropdownMenuItem>View details</DropdownMenuItem>
                                                <DropdownMenuItem>Edit</DropdownMenuItem>
                                                <DropdownMenuItem>Delete</DropdownMenuItem>
                                                <DropdownMenuSeparator></DropdownMenuSeparator>
                                                <DropdownMenuItem>Download receipt</DropdownMenuItem>
                                                </DropdownMenuContent>
                                            </DropdownMenu>
                                            </TableCell>
                                </TableRow> 
                                )
                            })}                        
                            </TableBody>
                        </Table>  
                    </div>                  
                </div>
            )}            
        </>
    )
}