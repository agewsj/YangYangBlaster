package gamedata

type TestTable struct {
	ID      int "index"
	Name    string
	HP      int
	LevelAP string
}

var (
	TestTableData = make(map[int]TestTable)
)

func TestTableinit() {
	rf := readRf(TestTable{})
	for i := 0; i < rf.NumRecord(); i++ {
		r := rf.Record(i).(*TestTable)
		TestTableData[r.ID] = *r
	}
}

func GetDataByID(id int) TestTable {
	return TestTableData[id]
}
