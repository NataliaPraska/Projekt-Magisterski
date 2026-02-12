SRC = magisterska.tex
AUX = magisterska

magisterska.pdf: $(SRC)
	pdflatex $<
	pdflatex $<
	biber $(AUX)
	pdflatex $<
	pdflatex $<

clean:
	rm -f *~ *.{pdf,log,lof,lol,lot,aux,blg,bbl,bcf,toc,nav,out,snm,xml}

