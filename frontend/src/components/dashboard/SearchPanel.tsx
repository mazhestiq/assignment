import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { apiClient } from "@/lib/api-client";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
import { Label } from "@/components/ui/label";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";
import { Calendar } from "@/components/ui/calendar";
import { CalendarIcon, Search } from "lucide-react";
import { format } from "date-fns";
import { cn } from "@/lib/utils";
import { SearchFilters } from "@/pages/Dashboard";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

interface SearchPanelProps {
  onSearch: (filters: SearchFilters) => void;
}

export const SearchPanel = ({ onSearch }: SearchPanelProps) => {
  const [selectedHotels, setSelectedHotels] = useState<string[]>([]);
  const [monthYear, setMonthYear] = useState<Date>();
  const [onlyLoyal, setOnlyLoyal] = useState(false);

  const { data: hotels } = useQuery({
    queryKey: ["hotels"],
    queryFn: () => apiClient.getHotels(),
  });

  const handleSearch = () => {
    onSearch({
      hotelIds: selectedHotels,
      monthYear,
      onlyLoyal,
    });
  };

  const toggleHotel = (hotelId: string) => {
    setSelectedHotels((prev) =>
      prev.includes(hotelId) ? prev.filter((id) => id !== hotelId) : [...prev, hotelId]
    );
  };

  return (
    <div className="space-y-6">
      <h2 className="text-xl font-semibold">Search Filters</h2>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        <div className="space-y-2">
          <Label>Hotels</Label>
          <Popover>
            <PopoverTrigger asChild>
              <Button variant="outline" className="w-full justify-between">
                {selectedHotels.length > 0
                  ? `${selectedHotels.length} selected`
                  : "Select hotels"}
              </Button>
            </PopoverTrigger>
            <PopoverContent className="w-80 p-4">
              <div className="space-y-3">
                {hotels?.map((hotel) => (
                  <div key={hotel.id} className="flex items-center space-x-2">
                    <Checkbox
                      id={hotel.id}
                      checked={selectedHotels.includes(hotel.id)}
                      onCheckedChange={() => toggleHotel(hotel.id)}
                    />
                    <Label htmlFor={hotel.id} className="cursor-pointer flex-1">
                      {hotel.name}
                    </Label>
                  </div>
                ))}
              </div>
            </PopoverContent>
          </Popover>
        </div>

        <div className="space-y-2">
          <Label>Month/Year</Label>
          <Popover>
            <PopoverTrigger asChild>
              <Button
                variant="outline"
                className={cn(
                  "w-full justify-start text-left font-normal",
                  !monthYear && "text-muted-foreground"
                )}
              >
                <CalendarIcon className="mr-2 h-4 w-4" />
                {monthYear ? format(monthYear, "MM/yyyy") : "Select month"}
              </Button>
            </PopoverTrigger>
            <PopoverContent className="w-auto p-0">
              <Calendar
                mode="single"
                selected={monthYear}
                onSelect={setMonthYear}
                initialFocus
                className="pointer-events-auto"
              />
            </PopoverContent>
          </Popover>
        </div>

        <div className="space-y-2">
          <Label className="opacity-0">Actions</Label>
          <div className="flex items-center space-x-2 h-10">
            <Checkbox
              id="loyal"
              checked={onlyLoyal}
              onCheckedChange={(checked) => setOnlyLoyal(checked as boolean)}
            />
            <Label htmlFor="loyal" className="cursor-pointer">
              Only Loyal Customers
            </Label>
          </div>
        </div>
      </div>

      <Button onClick={handleSearch} className="gap-2">
        <Search className="h-4 w-4" />
        Search Visits
      </Button>
    </div>
  );
};