import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "@/lib/api-client";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Calendar } from "@/components/ui/calendar";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";
import { CalendarIcon } from "lucide-react";
import { format } from "date-fns";
import { cn } from "@/lib/utils";
import { toast } from "sonner";

interface VisitRegistrationModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  customerId: string;
}

export const VisitRegistrationModal = ({
  open,
  onOpenChange,
  customerId,
}: VisitRegistrationModalProps) => {
  const queryClient = useQueryClient();
  const [selectedHotel, setSelectedHotel] = useState("");
  const [visitDate, setVisitDate] = useState<Date>();

  const { data: hotels } = useQuery({
    queryKey: ["hotels"],
    queryFn: () => apiClient.getHotels(),
  });

  const createVisitMutation = useMutation({
    mutationFn: async () => {
      if (!selectedHotel || !visitDate) {
        throw new Error("Please select both hotel and date");
      }

      return apiClient.createVisit({
        customer_id: customerId,
        hotel_id: selectedHotel,
        visit_date: format(visitDate, "yyyy-MM-dd"),
      });
    },
    onSuccess: () => {
      toast.success("Visit registered successfully");
      queryClient.invalidateQueries({ queryKey: ["customer-visits", customerId] });
      queryClient.invalidateQueries({ queryKey: ["visits"] });
      setSelectedHotel("");
      setVisitDate(undefined);
      onOpenChange(false);
    },
    onError: (error: Error) => {
      toast.error(error.message || "Failed to register visit");
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    createVisitMutation.mutate();
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Register New Visit</DialogTitle>
          <DialogDescription>
            Record a new hotel visit for this customer
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="hotel">Hotel *</Label>
            <Select value={selectedHotel} onValueChange={setSelectedHotel}>
              <SelectTrigger id="hotel">
                <SelectValue placeholder="Select a hotel" />
              </SelectTrigger>
              <SelectContent>
                {hotels?.map((hotel) => (
                  <SelectItem key={hotel.id} value={hotel.id}>
                    {hotel.name}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          <div className="space-y-2">
            <Label>Visit Date *</Label>
            <Popover>
              <PopoverTrigger asChild>
                <Button
                  variant="outline"
                  className={cn(
                    "w-full justify-start text-left font-normal",
                    !visitDate && "text-muted-foreground"
                  )}
                >
                  <CalendarIcon className="mr-2 h-4 w-4" />
                  {visitDate ? format(visitDate, "PPP") : "Select date"}
                </Button>
              </PopoverTrigger>
              <PopoverContent className="w-auto p-0">
                <Calendar
                  mode="single"
                  selected={visitDate}
                  onSelect={setVisitDate}
                  initialFocus
                  className="pointer-events-auto"
                />
              </PopoverContent>
            </Popover>
          </div>

          <div className="flex justify-end gap-3 pt-4">
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Cancel
            </Button>
            <Button type="submit" disabled={createVisitMutation.isPending}>
              Register Visit
            </Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
};